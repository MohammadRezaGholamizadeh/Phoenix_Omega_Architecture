using AccessControlLayer.AccessControll.Actions.Base;
using AccessControlLayer.AccessControll.Contracts;
using AccessControlLayer.AccessControll.Contracts.Dto;
using AccessControlLayer.AccessControll.Domains;
using AccessControlLayer.AccessControll.Exceptions;
using AccessControlLayer.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Reflection;

namespace AccessControlLayer.AccessControll
{
    public class AccessControlAppService : AccessControlService
    {
        private readonly UserTokenService _userTokenService;
        private readonly AccessControlRepository _repository;
        private readonly IHttpContextAccessor _accessor;
        private readonly AccessControlUnitOfWork _unitOfWork;

        public AccessControlAppService(
            UserTokenService userTokenService,
            AccessControlRepository repository,
            IHttpContextAccessor accessor,
            AccessControlUnitOfWork unitOfWork)
        {
            _userTokenService = userTokenService;
            _repository = repository;
            _accessor = accessor;
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserResource(
                          string userId,
                          AddUserResourceAccessDto access)
        {
            await GaurdAgainstApplicationUserNotFound(userId);
            var actionTypesId = access.ActionTypes
                                      .DistinctBy(_ => _.ActionTypeId)
                                      .Select(_ => _.ActionTypeId);
            var actions = Assembly
                          .GetAssembly(typeof(AccessControlAppService))?
                          .GetTypes()
                          .Where(_ => actionTypesId
                                      .Any(a => a == _.GUID.ToString()))
                          .Select(_ => new
                          {
                              ActionTypeId = _.GUID.ToString(),
                              Details = Activator.CreateInstance(_)
                          });

            GaurdAgainstActionNotExist(
                actionTypesId.DistinctBy(_ => _).Count(),
                actions?.DistinctBy(_ => _.ActionTypeId).Count());

            var lastUserResource =
                await _repository.GetAllUserResourceByUserId(userId);

            var userResources =
                access.ActionTypes
                .Where(actionTypes => !lastUserResource
                                       .Any(_ => _.ActionTypeId
                                              == actionTypes.ActionTypeId))
                .Select(_ => new UserResource()
                {
                    Id = Guid.NewGuid().ToString(),
                    ActionTypeId = _.ActionTypeId,
                    TargetResourceId =
                              _.ResourceIds.Select(resourceId =>
                                               new ResourceId()
                                               {
                                                   TargetResourceId = resourceId
                                               })
                                           .ToList(),
                    UserId = userId,
                    ResourceTypeId =
                          (actions?.FirstOrDefault(
                                     s => s.ActionTypeId
                                     == _.ActionTypeId)
                                   ?.Details
                                     as AccessAttribute)?
                                     .ResourceType.GUID.ToString()!
                });

            var dependantResources =
                actions!.Select(_ => _.Details as AccessAttribute)
                        .SelectMany(_ => _.DependentActions)
                        .Select(_ => _.GUID.ToString())
                        .DistinctBy(_ => _);

            if (dependantResources
                .Any(dep => !actions!.Any(_ => _.ActionTypeId == dep)
                         && !lastUserResource.Any(_ => _.ActionTypeId == dep)))
                throw new SomeActionNotSelectedCorrectlyException();

            var totalResource = userResources.DistinctBy(_ => _.ActionTypeId);
            await _repository.AddRange(totalResource);
        }

        public List<GetActionTypeDto>? GetAllActionTypes()
        {
            var actions =
                Assembly.GetAssembly(typeof(AccessControlAppService))?
                        .GetTypes()
                        .Where(_ => _.BaseType == typeof(AccessAttribute))
                        .Select(_ =>
                            new GetActionTypeDto(
                                _.GUID.ToString(),
                                _.Name.Replace("Attribute", "")))
                        .ToList();
            return actions;
        }

        public async Task HasAccessOnResource(
                          object? resourceId = null)
        {
            var httpContextEndPoint =
                _accessor.HttpContext?.GetEndpoint();
            var request = httpContextEndPoint?.RequestDelegate?.Target;
            var action = request?.GetType()
                          ?.GetRuntimeField("actionDescriptor")
                          ?.GetValue(request) as ActionDescriptor;

            var actionMetadata =
                    action?.EndpointMetadata
                           .Single(_ =>
                                  _.GetType().BaseType
                                  == typeof(AccessAttribute));
            var mainActionTypeId =
                actionMetadata?.GetType().GUID.ToString().ToLower();

            var mainAction =
                new AccessActionDto()
                {
                    ActionTypeId = mainActionTypeId!,
                    ResourceTypeId =
                    (actionMetadata as AccessAttribute)?
                    .ResourceType.GUID.ToString().ToLower()!
                };

            var dependentActions =
                (actionMetadata as AccessAttribute)?
                .DependentActions
                .Select(_ =>
                   new AccessActionDto()
                   {
                       ActionTypeId = _.GUID.ToString().ToLower(),
                       ResourceTypeId =
                        (Activator.CreateInstance(_) as AccessAttribute)
                        ?.ResourceType.GUID.ToString().ToLower()!
                   })
                .ToArray();

            var hasAccess =
                _repository
                .CheckUserAccess(
                   _userTokenService.UserId,
                   mainAction,
                   dependentActions!,
                   resourceId?.ToString());

            if (!hasAccess)
            {
                throw new AccessDeniedException();
            }
        }

        public async Task DeleteUserResource(
            string userId,
            DeleteUserResourceDto dto)
        {
            await GaurdAgainstApplicationUserNotFound(userId);

            var deletingUserResource =
                await _repository.FindAllDeletingUserResources(
                                  userId,
                                  dto.DeletingActionTypeId);

            _repository.DeleteAll(deletingUserResource);
        }

        public async Task<List<GetApplicationUserReource>>
            GetAllUserResourceByUserId(string userId)
        {
            return await
                _repository.GetAllUserResourceByUserId(
                            userId);
        }

        public async Task GenerateAccessForUser(object targetId)
        {
            var httpContextEndPoint =
                _accessor.HttpContext?.GetEndpoint();
            var request = httpContextEndPoint?.RequestDelegate?.Target;
            var action = request?.GetType()
                          ?.GetRuntimeField("actionDescriptor")
                          ?.GetValue(request) as ActionDescriptor;

            var actionMetadata =
                    action?.EndpointMetadata
                           .Single(_ =>
                                  _.GetType().BaseType
                                  == typeof(AccessAttribute));

            var assignableActions =
                 (actionMetadata as AccessAttribute)?
                 .AssignableActions
                 .Select(_ =>
                    new AccessActionDto()
                    {
                        ActionTypeId = _.GUID.ToString().ToLower(),
                        ResourceTypeId =
                         (Activator.CreateInstance(_) as AccessAttribute)
                         !.ResourceType.GUID.ToString().ToLower()
                    })
                 .ToArray();

            var mainActionTypeId =
                actionMetadata?.GetType().GUID.ToString().ToLower();

            var assignableActionTypesId =
                assignableActions!.Select(_ => _.ActionTypeId)
                                  .Append(mainActionTypeId);

            var userResources = await _repository
                           .GetAllUserResourceByUserIdAndActionTypeId(
                            assignableActionTypesId!,
                            _userTokenService.UserId);

            userResources.Where(_ => _.TargetResourceId.Any())
                .ToList().ForEach(_ => _.TargetResourceId.Add(new ResourceId()
                {
                    Id = Guid.NewGuid().ToString(),
                    TargetResourceId = targetId.ToString()!
                }));

            var newAction =
                assignableActions!
                .Where(assignableAction =>
                       !userResources.Any() ||
                       !userResources.Any(_ => _.ActionTypeId
                                == assignableAction.ActionTypeId))
                .Select(_ => new UserResource()
                {
                    Id = Guid.NewGuid().ToString(),
                    ActionTypeId = _.ActionTypeId,
                    ResourceTypeId = _.ResourceTypeId,
                    TargetResourceId = new List<ResourceId>()
                    {
                        new ResourceId()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TargetResourceId = targetId.ToString()!
                        }
                    },
                    UserId = _userTokenService.UserId
                });

            await _repository.AddRange(newAction);
            await _unitOfWork.Complete();
        }

        public async Task<List<GetUserAccessResultDto>>
            GetAllUserAccessResultByActionTypeId(
            AllActionTypeIdDto allActionTypeIdDto)
        {
            var actionTypesId = allActionTypeIdDto.ActionTypesId;
            return await _repository.GetAllUserAccessResultByActionTypeId(
                                     _userTokenService.UserId,
                                     actionTypesId);
        }

        public async Task GenerateAccessForApplicationUserRole(
            List<string> roleIds,
            string userId)
        {
            await GuadrAganistRoleNotExist(roleIds);
            List<GetRoleActionDto> allRoleAction =
                await _repository.GetAllRoleActionByRoleId(roleIds);
            var actions =
                 typeof(AccessAttribute)
                 .Assembly
                 .GetTypes()
                 .Where(_ => _.BaseType == typeof(AccessAttribute)
                          && allRoleAction
                             .Any(r => r.ActionTypeId == _.GUID.ToString()))
                 .ToList();
            GuardAgainstActionNotExist(
                allRoleAction.DistinctBy(_ => _.ActionTypeId).Count(),
                actions.Count);

            var userResource =
                allRoleAction.Select(_ => new UserResource()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ActionTypeId = _.ActionTypeId,
                    RoleId = _.RoleId,
                    ResourceTypeId =
                    (Activator.CreateInstance(
                         actions.Single(a => a.GUID.ToString().ToLower()
                                          == _.ActionTypeId.ToLower()))
                     as AccessAttribute)!.ResourceType.GUID.ToString()
                });

            await _repository.AddRange(userResource);
        }

        public async Task GenerateAccessForApplicationUserRoleByTav(
    List<string> roleIds,
    string userId,
    string tenantId)
        {
            await GuadrAganistRoleNotExist(roleIds);
            List<GetRoleActionDto> allRoleAction =
                await _repository.GetAllRoleActionByRoleId(roleIds);
            var actions =
                 typeof(AccessAttribute)
                 .Assembly
                 .GetTypes()
                 .Where(_ => _.BaseType == typeof(AccessAttribute)
                          && allRoleAction
                             .Any(r => r.ActionTypeId == _.GUID.ToString()))
                 .ToList();
            GuardAgainstActionNotExist(allRoleAction.Count, actions.Count);

            var userResource =
                allRoleAction.Select(_ => new UserResource()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ActionTypeId = _.ActionTypeId,
                    RoleId = _.RoleId,
                    ResourceTypeId =
                    (Activator.CreateInstance(
                         actions.Single(a => a.GUID.ToString().ToLower()
                                          == _.ActionTypeId.ToLower()))
                     as AccessAttribute)!.ResourceType.GUID.ToString(),
                    TenantId = tenantId
                });

            await _repository.AddRange(userResource);

        }

        public async Task DeleteAccessForApplicationUserRole(
            List<string> deletedRoles,
            string userId)
        {
            var deletingUserRoleAction =
                await _repository.GetAllUserResourceByRolesIdAndUserId(
                      deletedRoles,
                      userId);

            _repository.DeleteAll(deletingUserRoleAction);
            _unitOfWork.CommitPartial();
        }

        private async Task GaurdAgainstApplicationUserNotFound(string userId)
        {
            if (!await _repository.IsExistApplicationUserByUserId(
                                    userId))
                throw new UserNotFoundException();
        }

        private static void GaurdAgainstActionNotExist(
            int actionTypesCount,
            int? actionsCount)
        {
            if (actionsCount != actionTypesCount)
                throw new AccessControlActionNotExistException();
        }

        private async Task GuardAgainstDuplicateAction(
                string? userId,
                IEnumerable<string> actionTypesId)
        {
            if (await _repository
                      .IsDuplicateActionForUser(
                        userId!,
                        actionTypesId))
                throw new DuplicateAccessControlActionException();
        }
        private static void GuardAgainstActionNotExist(
            int allRoleActionCount,
            int actionsCount)
        {
            if (actionsCount != allRoleActionCount)
                throw new ActionNotFoundException();
        }

        private async Task GaurdAgainstUserNotExist(string userId)
        {
            if (!await _repository.IsExistUserByUserId(userId))
                throw new UserNotExistException();
        }

        private async Task GuadrAganistRoleNotExist(List<string> roleIds)
        {
            if (!await _repository.IsAllRoleExist(roleIds))
                throw new RoleNotExistException();
        }

    }
}
