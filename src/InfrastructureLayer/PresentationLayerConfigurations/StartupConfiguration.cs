using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfrastructureLayer.ConfigurationsJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
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
                .UseSwaggerAndSwaggerUI()
                .InitializeDataBase();
        }
        public static void ConfigureContainer(ContainerBuilder builder)
        {
        }

        public static IHost CreateHostWithAutofacConfig(
        string[] args,
        Type startupType)
        {
            var hostBuilder =
                Host.CreateDefaultBuilder(args)
                .UseSerilog(
                    (_, config) =>
                    {
                        config.WriteTo
                                 .Console(
                                    theme: AnsiConsoleTheme.Literate
                                    //outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj} {Properties:j}{NewLine}"
                                    )
                              .ReadFrom
                                 .Configuration(_.Configuration);
                    })
                .UseServiceProviderFactory(
                    new AutofacServiceProviderFactory());

            ConfigurationJson.BindConfigurationJsons(hostBuilder);

            hostBuilder
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseKestrel();
                     webBuilder.UseIISIntegration();
                     webBuilder.UseStartup(startupType)
                               .UseContentRoot(Directory.GetCurrentDirectory());
                 });
            return hostBuilder.Build();
        }
    }
}
