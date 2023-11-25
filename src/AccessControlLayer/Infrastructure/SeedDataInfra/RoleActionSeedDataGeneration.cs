using AccessControlLayer.Infrastructure.SeedDataInfra.Contracts;

namespace AccessControlLayer.Infrastructure.SeedDataInfra
{
    public abstract class RoleActionSeedDataGeneration
    {
        private readonly RoleActionSeedDataRepository _repository;
        private readonly AccessControlUnitOfWork _unitOfWork;

        protected RoleActionSeedDataGeneration(
            RoleActionSeedDataRepository repository,
            AccessControlUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public abstract void Up();
        public abstract void Down();

        public UpdateRoleActionConfiguration Update =>
               new UpdateRoleActionConfiguration(
                   _repository,
                   _unitOfWork);
    }
}
