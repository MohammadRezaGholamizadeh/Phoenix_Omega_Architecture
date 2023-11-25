using AccessControlLayer.AccessControll.Actions.Base;
using AccessControlLayer.AccessControll.Domains;

namespace AccessControlLayer.Infrastructure.SeedDataInfra
{
    public static class RoleActionSeedDataRunner
    {
        public static async Task Run(this UpdateRoleAction obj)
        {
            var roleId = await obj.Repository
                                  .GetRoleIdByRoleName(obj.RoleName);
            GuardAgainstRoleNotExist(roleId);

            await DeleteRoleActions(obj, roleId);
            await AddRoleActions(obj, roleId);
        }

        private static async Task AddRoleActions(
            UpdateRoleAction obj, string? roleId)
        {
            if (obj.NewActionTypesId.Any())
            {
                var notRegisteredNewActions =
               obj.Repository
                  .GetAllRoleActionThatNotRegistered(
                   obj.NewActionTypesId, roleId!);

                var roleActions =
                    notRegisteredNewActions
                    .Select(_ => new RoleAction()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = roleId!,
                        ActionTypeId = _.ToString()
                    });

                var actionTypesDetail =
                    typeof(AccessAttribute).Assembly
                    .GetTypes()
                    .Where(_ => _.BaseType == typeof(AccessAttribute)
                             && roleActions
                                .Any(roleAction => roleAction.ActionTypeId
                                                   == _.GUID.ToString()))
                    .Select(_ => new
                    {
                        ActionType = _.GUID.ToString(),
                        Details = Activator.CreateInstance(_) as AccessAttribute
                    });

                var ApplicationUserRoles =
                    await obj.Repository
                             .GetAllApplicationUserRoleByRoleId(roleId);

                List<UserResource> userResources
                    = new List<UserResource>();

                ApplicationUserRoles.ForEach(pur =>
                     userResources.AddRange(
                         actionTypesDetail.Select(_ =>
                                            new UserResource()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                ActionTypeId = _.ActionType,
                                                RoleId = roleId,
                                                //TenantId = pur.TenantId,
                                                UserId = pur.UserId,
                                                ResourceTypeId =
                                                    _.Details!
                                                    .ResourceType.GUID
                                                    .ToString()
                                            })));

                obj.Repository.AddUserResources(userResources);
                obj.Repository.AddRangeRoleActions(roleActions);
                obj.UnitOfWork.CommitPartial();
            }
        }

        private static async Task DeleteRoleActions(
            UpdateRoleAction obj,
            string? roleId)
        {
            if (obj.DeletedActionTypesId.Any())
            {
                List<RoleAction> roleActions =
                await obj.Repository.GetAllRoleActionByRoleId(roleId!);

                var actionTypes =
                  typeof(AccessAttribute)
                  .Assembly
                  .GetTypes()
                  .Where(_ => _.BaseType == typeof(AccessAttribute)
                           && roleActions
                              .Any(r => r.ActionTypeId == _.GUID.ToString()
                           && !obj.DeletedActionTypesId
                                  .Contains(_.GUID.ToString())))
                  .Select(_ => Activator.CreateInstance(_) as AccessAttribute)
                  .ToList();

                var dependentActions =
                    actionTypes
                    .SelectMany(_ => _.DependentActions)
                    .Select(_ => _.GUID.ToString())
                    .ToList();

                var validActionsForDelete =
                    obj.DeletedActionTypesId.AsParallel()
                       .Where(_ => !dependentActions
                                   .AsParallel()
                                   .Any(actionType =>
                                        actionType == _))
                       .ToList();
                var deletingRoleAction =
                    roleActions
                    .AsParallel()
                    .Where(_ => validActionsForDelete.AsParallel()
                                .Contains(_.ActionTypeId))
                    .ToList();

                List<UserResource> UserResources =
                 await obj.Repository
                          .FindAllUserResourceByRoleAndActionTypeId(
                           validActionsForDelete, roleId!);

                obj.Repository
                   .DeleteUserResources(UserResources);
                obj.Repository.RemoveAll(deletingRoleAction);
                obj.UnitOfWork.CommitPartial();
            }
        }

        private static async Task GuardAgainstDuplicateRoleAction(
            UpdateRoleAction obj,
            string? roleId)
        {
            if (await obj.Repository
                         .IsExistDuplicateRoleAction(
                            roleId,
                            obj.NewActionTypesId))
                throw new Exception("RoleActionExist");
        }

        private static void GuardAgainstRoleNotExist(string? roleId)
        {
            if (roleId == null)
                throw new Exception("RoleIdNotFound");
        }
    }
}
