using InfrastructureLayer.DataAccessConfigurations;
using InfrastructureLayer.PresentationLayerConfigurations;
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
                        Path.GetDirectoryName(
                             typeof(StartupConfiguration)
                             .Assembly
                             .Location) + @"\ConfigurationsJson";

                    config.SetBasePath(path)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
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
