using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace InfrastructureLayer.PresentationLayerConfigurations
{
    public static class StartupConfiguration
    {
        public static void ConfigureServices(
            IServiceCollection services,
            Assembly presentationAssembly)
        {
            new ServiceCollectionConfigurationSetter(services)
                .AddAutofac()
                .AddCors()
                .AddRouting()
                .AddHealthChecks()
                .AddControllers()
                .AddHttpContextAccessor()
                .AddMvcCore(presentationAssembly)
                .AddAuthorization()
                .AddApiVersioning()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
        }

        public static void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            new ApplicationBuilderConfigurationSetter(app)
                .UseCors()
                .UseRouting()
                .UseEndpointsToConfigHealthCheck()
                .UseAuthorization()
                .UseAuthentication()
                .UseEndpointsToMapControllers()
                .UseHealthChecks()
                .UseHttpsRedirection()
                .UseSwaggerAndSwaggerUI();
        }
        public static void ConfigureContainer(ContainerBuilder builder)
        {
        }

        public static IHost CreateHostWithAutofacConfig(
        string[] args,
        object auofacConfigInstance,
        Type containerType,
        Type startupType)
        {
            var hostBuilder =
                Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(
                        new AutofacServiceProviderFactory());

            hostBuilder
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseKestrel();
                     webBuilder.UseIISIntegration();
                     webBuilder.UseStartup(startupType)
                               .UseContentRoot(Directory.GetCurrentDirectory());
                 });

            hostBuilder.ConfigureContainer<ContainerBuilder>(
                containerBuilder =>
                {
                    containerType
                     .GetMethod("Configure")?
                     .Invoke(
                        auofacConfigInstance,
                        new object[] { containerBuilder });
                });

            return hostBuilder.Build();
        }
    }
}
