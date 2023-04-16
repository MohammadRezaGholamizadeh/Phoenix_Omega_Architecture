using Autofac;
using ConfigurationLayer.InfrastructureLayerConfiguration.AutoFacConfigurations;
using InfrastructureLayer.PresentationLayerConfigurations;

namespace PresentationLayer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            StartupConfiguration.ConfigureServices(
                services,
                typeof(Program).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StartupConfiguration.Configure(app, env);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            StartupConfiguration.ConfigureContainer(builder);
        }

        public IHost CreateHostBuilderWithAutofacConfig(
        string[] args,
        object autoFacConfigInstance)
        {
            return StartupConfiguration.CreateHostWithAutofacConfig(
               args,
               autoFacConfigInstance,
               typeof(AutofacConfig),
               typeof(Startup));
        }
    }
}