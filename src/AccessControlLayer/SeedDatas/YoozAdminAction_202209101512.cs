using AccessControlLayer.AccessControll.Actions.LogicalActions.RoleActions;
using AccessControlLayer.AccessControll.Actions.LogicalActions.UserActions;
using AccessControlLayer.Infrastructure;
using AccessControlLayer.Infrastructure.SeedDataInfra;
using AccessControlLayer.Infrastructure.SeedDataInfra.Contracts;
using IdentityLayer.AspDotNetIdentity.Domain;

namespace AccessControlLayer.SeedDatas
{
    [SeedDataVersion(202209101512)]
    public class YoozAdminAction_202209101512 : RoleActionSeedDataGeneration
    {
        public YoozAdminAction_202209101512(
            RoleActionSeedDataRepository repository,
            AccessControlUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public override void Up()
        {
            Update
            .Role(SystemRoles.Admin)
            .AddAction<GetAllUserAttribute>()
            .AddAction<GetUserAttribute>()
            .AddAction<GetAllRoleAttribute>()
            .Generate()
            .Run()
            .Wait();
        }

        public override void Down()
        {
            Update
           .Role(SystemRoles.Admin)
           .DeleteAction<GetAllUserAttribute>()
           .DeleteAction<GetUserAttribute>()
           .DeleteAction<GetAllRoleAttribute>()
           .Generate()
           .Run()
           .Wait();
        }
    }
}
