using InfrastructureLayer.DataAccessConfigurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace InfrastructureLayer.ConfigurationsJson
{
    public static class ConfigurationJson
    {
        public static IHostBuilder BindConfigurationJsons(
           this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(
                (context, config) =>
                {
                    var path =
                        Directory.GetCurrentDirectory() + @"\ConfigurationsJson";

                    config.SetBasePath(path)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                          .AddJsonFile("BackgroundJobConfigurations.json", optional: true, reloadOnChange: true)
                          .AddEnvironmentVariables()
                          .Build();
                });
        }

        public static DataAccessConfig GetDataAccessConfig(
           this IConfiguration configuration)
        {
            var dataAccessConfig = new DataAccessConfig();
            configuration.Bind(nameof(DataAccessConfig), dataAccessConfig);
            return dataAccessConfig;
        }
    }
}
