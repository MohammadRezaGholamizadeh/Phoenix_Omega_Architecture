using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace InfrastructureLayer.PresentationLayerConfigurations
{
    public static class EnvironmentSetting
    {
        public static void SetHostUrlsAndEnvironment(this IWebHostBuilder webHostBuilder)
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory() + "/ConfigurationsJson")
                                    .AddJsonFile("appsettings.json")
                                    .AddEnvironmentVariables()
                                    .Build();

            webHostBuilder.UseEnvironment(configuration.GetValue<string>("Environment" , defaultValue: "Development"));
            webHostBuilder.UseUrls(configuration.GetValue<string>("HostUrls"));
        }
    }
}
