using AccessControlLayer.AccessControll.Actions.Base;
using AccessControlLayer.Infrastructure.SeedDataInfra.Contracts;

namespace AccessControlLayer.Infrastructure.SeedDataInfra
{
    public class UpdateRoleActionConfiguration
    {
        private readonly UpdateRoleAction updateRole;

        public UpdateRoleActionConfiguration(
            RoleActionSeedDataRepository context,
            AccessControlUnitOfWork unitOfWork)
        {
            updateRole = new UpdateRoleAction(context, unitOfWork);
        }

        public UpdateRoleActionConfiguration Role(
          string roleName)
        {
            updateRole.RoleName = roleName;
            return this;
        }

        public UpdateRoleActionConfiguration AddAction<T>()
        {
            var mainActionType = typeof(T);
            updateRole.NewActionTypesId.Add(mainActionType.GUID.ToString());
            var dependentActions =
                Activator.CreateInstance(mainActionType)
                as AccessAttribute;
            updateRole.NewActionTypesId
                       .AddRange(dependentActions!
                                 .DependentActions
                                 .Select(_ => _.GUID.ToString()));
            return this;
        }

        public UpdateRoleActionConfiguration DeleteAction<T>()
        {
            var mainActionType = typeof(T);
            updateRole.DeletedActionTypesId.Add(mainActionType.GUID.ToString());
            var dependentActions =
                Activator.CreateInstance(mainActionType)
                as AccessAttribute;
            updateRole.DeletedActionTypesId
                       .AddRange(dependentActions!
                                 .DependentActions
                                 .Select(_ => _.GUID.ToString()));
            return this;
        }

        public UpdateRoleAction Generate()
        {
            updateRole.NewActionTypesId =
                updateRole.NewActionTypesId.Distinct().ToList();
            updateRole.DeletedActionTypesId =
                updateRole.DeletedActionTypesId.Distinct().ToList();
            return updateRole;
        }
    }
}
