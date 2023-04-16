using Autofac;
using ConfigurationLayer.InfrastructureLayerConfiguration.AutoFacConfigurations;
using InfrastructureLayer.PresentationLayerConfigurations;

namespace PresentationLayer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            StartupConfiguration.ConfigureServices(
                services,
                typeof(Program).Assembly);
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            StartupConfiguration.Configure(app, env);
        }

        public void ConfigureContainer(
            ContainerBuilder builder)
        {
            AutofacConfig
                .Configure(builder, _configuration);
        }
    }
}