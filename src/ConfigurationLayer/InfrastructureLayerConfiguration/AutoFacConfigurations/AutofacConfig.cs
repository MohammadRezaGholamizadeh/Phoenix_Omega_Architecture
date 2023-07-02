using ApplicationLayer.AppliactionServices.ColorsAppService;
using ApplicationLayer.ApplicationServiceInterface;
using ApplicationLayer.InfraInterfaces.UnitOfWorks;
using ApplicationLayer.ServiceInterface;
using Autofac;
using DataAccessLayer.EFTech.EFDataContexts;
using DataAccessLayer.EFTech.EFRepositories.Colors;
using DataAccessLayer.EFTech.UnitOfWorks;
using IdentityLayer.AspDotNetIdentity.Services;
using IdentityLayer.AspDotNetIdentity.Services.Contracts;
using InfrastructureLayer.ConfigurationsJson;
using InfrastructureLayer.MigrationLayerConfigurations.Contracts;
using MigrationLayer;
using ServiceLayer.RepositoryInterface;
using ServiceLayer.Services.ColorService;

namespace ConfigurationLayer.InfrastructureLayerConfiguration.AutoFacConfigurations
{
    public static class AutofacConfig
    {
        public static ContainerBuilder Configure(
            ContainerBuilder builder,
            IConfiguration configuration)
        {
            SystemRequirementService(builder, configuration);
            var dataAccessConfig = configuration.GetDataAccessConfig();

            builder.RegisterAssemblyTypes(typeof(ColorAppService).Assembly)
                  .AssignableTo<IApplicationService>()
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ColorService).Assembly,
                                          typeof(IdentityAppService).Assembly)
                   .AssignableTo<IService>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(EFColorRepository).Assembly)
                      .AssignableTo<IRepository>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<EFDataContext>()
                   .AsSelf()
                   .WithParameter("connectionString",
                                  dataAccessConfig.ConnectionString)
                   .InstancePerLifetimeScope();

            builder.RegisterType<EFUnitOfWork>()
                   .As<UnitOfWork>()
                   .InstancePerLifetimeScope();

            return builder;
        }

        private static void SystemRequirementService(
            ContainerBuilder builder,
            IConfiguration configuration)
        {

            builder.RegisterType<MigrationRunner>()
                .As<IMigrationRunner>()
                .SingleInstance();
        }
    }
}
