using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfrastructureLayer.PresentationLayerConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        new ServiceCollectionConfigurationSetter(services)
            .AddAutofac()
            .AddCors()
            .AddRouting()
            .AddHealthChecks()
            .AddRouting()
            .AddControllers()
            .AddHttpContextAccessor()
            .AddMvcCore()
            .AddAuthorization()
            .AddRouting()
            .AddSwaggerGen()
            .AddApiVersioning()
            .AddEndpointsApiExplorer();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        new ApplicationBuilderConfigurationSetter(app)
            .UseCors()
            .UseRouting()
            .UseEndpointsToConfigHealthCheck()
            .UseRouting()
            .UseAuthorization()
            .UseAuthentication()
            .UseEndpointsToMapControllers()
            .UseHealthChecks()
            .UseHttpsRedirection()
            .UseSwaggerAndSwaggerUI();
    }
    public void ConfigureContainer(ContainerBuilder builder)
    {
    }

    public IHostBuilder CreateHostBuilderWithAutofacConfig<T>(
    string[] args,
    object containerBuilderInstance) where T : class =>
       Host.CreateDefaultBuilder(args)
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseKestrel();
               webBuilder.UseIISIntegration();
               webBuilder.UseStartup<Startup>()
                         //.UseContentRoot(Directory.GetCurrentDirectory())
                         .ConfigureServices(_ =>
                         {
                             var containerBuilder = new ContainerBuilder();
                             containerBuilder.Populate(_);

                             typeof(T).GetType()
                                      .GetMethod("Configure")?
                                      .Invoke(containerBuilderInstance,
                                              new object[] { containerBuilder });

                             containerBuilder.Build();
                         });
           });
}
