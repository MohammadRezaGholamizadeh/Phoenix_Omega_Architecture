using AccessControlLayer.AccessControll.Contracts;
using AccessControlLayer.AccessControll.Contracts.Dto;
using AccessControlLayer.AccessControll.Domains;
using DataAccessLayer.EFTech.EFDataContexts;
using IdentityLayer.AspDotNetIdentity.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EFTech.AccessControls.Base
{
    public class EFAccessControlRepository : AccessControlRepository
    {
        private readonly EFDataContext _context;

        public EFAccessControlRepository(EFDataContext context)
        {
            _context = context;
        }

        public async Task
            AddRange(IEnumerable<UserResource> resources)
        {
            await _context.UserResources.AddRangeAsync(resources);
        }

        private static bool CheckResourceIdCondition(
            List<ResourceId> targetsResourceId,
            string? resourceId = null)
        {
            if (resourceId != null
                && targetsResourceId.Any())
            {
                return targetsResourceId
                       .Any(_ => _.TargetResourceId
                                  == resourceId);
            }

            return true;
        }

        public bool CheckUserAccess(
                    string oauthUserId,
                    AccessActionDto mainAction,
                    AccessActionDto[] dependentActions,
                    string? resourceId)
        {
            return _context.UserResources
                   .Include(_ => _.User)
                   .Include(_ => _.TargetResourceId)
                   .AsParallel()
                   .Where(userResource =>
                            userResource.User.UserName
                             == oauthUserId
                          && (mainAction.ActionTypeId
                                == userResource.ActionTypeId.ToLower()
                               && mainAction.ResourceTypeId
                                  == userResource.ResourceTypeId.ToLower()
                               && CheckResourceIdCondition(
                                  userResource.TargetResourceId,
                                  resourceId)
                          ||
                             dependentActions.Any(_ =>
                             _.ActionTypeId == userResource.ActionTypeId
                               && _.ResourceTypeId
                                  == userResource.ResourceTypeId)))
                   .DistinctBy(_ => _.ActionTypeId)
                   .Count() == dependentActions
                               .Append(mainAction)
                               .Select(_ => _.ActionTypeId)
                               .DistinctBy(_ => _)
                               .Count();
        }

        public void DeleteAll(
            IEnumerable<UserResource> deletingUserResource)
        {
            _context.UserResources
                    .RemoveRange(deletingUserResource);
        }

        public async Task<List<UserResource>>
            FindAllDeletingUserResources(
            string userId,
            List<string> deleteingUserResourcesId)
        {
            return await _context
                         .UserResources
                         .Where(_ => _.UserId == userId
                                  && deleteingUserResourcesId
                                     .Contains(_.ActionTypeId))
                         .ToListAsync();
        }

        public async Task<List<GetApplicationUserReource>>
            GetAllUserResourceByUserId(string userId)
        {
            return await
                _context.UserResources
                        .Where(_ => _.UserId == userId)
                        .Select(_ =>
                           new GetApplicationUserReource(_.ActionTypeId)
                           {
                               ResourcesId =
                                  _.TargetResourceId
                                   .Select(_ => _.TargetResourceId)
                                   .ToList()
                           })
                        .ToListAsync();
        }

        public async Task<string?> GetApplicationUserIdByUserId(
            string userId)
        {
            var applicationUserId =
                await _context.Set<ApplicationUser>()
                              .Where(_ => _.Id == userId)
                              .SingleOrDefaultAsync();
            return applicationUserId?.Id;
        }

        public async Task<bool> IsDuplicateActionForUser(
            string userId,
            IEnumerable<string> actionTypesId)
        {
            return await
                _context.UserResources
                        .Where(_ => _.UserId == userId
                                 && _.RoleId == null)
                        .AnyAsync(_ => actionTypesId
                                       .Contains(_.ActionTypeId));
        }

        public async Task<bool> IsExistApplicationUserByUserId(string userId)
        {
            return await _context.Users
                                 .AnyAsync(_ => _.Id == userId);
        }

        public async Task<List<UserResource>>
            GetAllUserResourceByUserIdAndActionTypeId(
            IEnumerable<string> assignableActions,
            string userId)
        {
            return await _context.UserResources
                                 .Include(_ => _.TargetResourceId)
                                 .Where(pur =>
                                        pur.UserId == userId
                                        && assignableActions!
                                           .Any(_ => _ == pur.ActionTypeId))
                                 .ToListAsync();
        }

        public async Task<List<GetUserAccessResultDto>>
            GetAllUserAccessResultByActionTypeId(
            string oauthUserId,
            List<string> actionTypesId)
        {
            var accessResults = await _context.UserResources
                                 .Where(_ => _.User.UserName == oauthUserId
                                          && actionTypesId
                                             .Contains(_.ActionTypeId))
                                 .Select(_ => _.ActionTypeId)
                                 .ToListAsync();

            return actionTypesId
                   .Select(_ => new GetUserAccessResultDto()
                   {
                       ActionTypesId = _,
                       HasAccess = accessResults.Any(result => result == _)
                   }).ToList();
        }

        public async Task<bool> IsAllRoleExist(List<string> roleIds)
        {
            return await _context.Set<ApplicationRole>()
                .AnyAsync(_ => !roleIds.Contains(_.Id));
        }

        public async Task<bool> IsExistUserByUserId(string userId)
        {
            return await _context.Users.AnyAsync(_ => _.Id == userId);
        }

        public async Task<List<GetRoleActionDto>>
            GetAllRoleActionByRoleId(List<string> roleIds)
        {
            return
                await _context.Set<RoleAction>()
                              .Where(_ => roleIds
                                          .Contains(_.RoleId))
                              .Select(_ => new GetRoleActionDto()
                              {
                                  RoleId = _.RoleId,
                                  ActionTypeId = _.ActionTypeId
                              })
                              .ToListAsync();
        }

        public async Task<List<UserResource>>
            GetAllUserResourceByRolesId(List<string> rolesId)
        {
            return await
                _context.UserResources
                        .Where(_ => _.RoleId != null
                                 && rolesId.Contains(_.RoleId))
                        .ToListAsync();
        }

        public async Task<List<UserResource>>
            GetAllUserResourceByRolesIdAndUserId(
            List<string> rolesId,
            string userId)
        {
            return await
                  _context.UserResources
                          .Where(_ => _.RoleId != null
                                   && rolesId.Contains(_.RoleId)
                                   && _.UserId == userId)
                          .ToListAsync();
        }
    }
}
