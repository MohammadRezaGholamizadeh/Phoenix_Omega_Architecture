using ApplicationLayer.AppliactionServices.ColorsAppService;
using ApplicationLayer.ApplicationServiceInterface;
using ApplicationLayer.InfraInterfaces.UnitOfWorks;
using ApplicationLayer.ServiceInterface;
using Autofac;
using DataAccessLayer.EFTech.EFDataContexts;
using DataAccessLayer.EFTech.EFRepositories.Colors;
using DataAccessLayer.EFTech.UnitOfWorks;
using ServiceLayer.RepositoryInterface;
using ServiceLayer.Services.ColorService;

namespace ConfigurationLayer.InfrastructureLayerConfiguration.AutoFacConfigurations
{
    public class AutofacConfig
    {
        public void Configure(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ColorAppService).Assembly)
                  .AssignableTo<IApplicationService>()
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ColorService).Assembly)
                   .AssignableTo<IService>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(EFColorRepository).Assembly)
                      .AssignableTo<IRepository>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<EFDataContext>()
                   .AsSelf()
                   .WithParameter("connectionString", "server=.;database=Yooz-DemoStage;Trusted_Connection = true")
                   .InstancePerLifetimeScope();

            builder.RegisterType<EFUnitOfWork>()
                   .As<UnitOfWork>()
                   .InstancePerLifetimeScope();
        }
    }
}
