using AccessControlLayer.Infrastructure.SeedDataInfra.Contracts;

namespace AccessControlLayer.Infrastructure.SeedDataInfra
{
    public class UpdateRoleAction
    {
        public UpdateRoleAction(
            RoleActionSeedDataRepository repository,
            AccessControlUnitOfWork unitOfWork)
        {
            NewActionTypesId = new List<string>();
            DeletedActionTypesId = new List<string>();
            Repository = repository;
            UnitOfWork = unitOfWork;
        }

        public RoleActionSeedDataRepository Repository { get; set; }
        public AccessControlUnitOfWork UnitOfWork { get; set; }
        public string RoleName { get; set; }
        public List<string> NewActionTypesId { get; set; }
        public List<string> DeletedActionTypesId { get; set; }
    }
}
