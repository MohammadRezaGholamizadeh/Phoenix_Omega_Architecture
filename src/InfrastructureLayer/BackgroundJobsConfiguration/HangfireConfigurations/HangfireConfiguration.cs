using Hangfire;
using Hangfire.SQLite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureLayer.BackgroundJobsConfiguration.HangfireConfigurations
{
    public static class HangfireConfiguration
    {
        public static IServiceCollection AddHangfireConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var hangfireConfig =
                configuration.GetHangfireConfiguration();

            services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                             .UseSimpleAssemblyNameTypeSerializer()
                             .UseRecommendedSerializerSettings()
                             .UseCustomStorage(hangfireConfig);
            });
            return services.AddHangfireServer();
        }

        private static IGlobalConfiguration UseCustomStorage(
            this IGlobalConfiguration config,
            HangfireConfig dataAccessConfiguration)
        {
            switch (dataAccessConfiguration.DBProvider.ToLowerInvariant())
            {
                case "sqlserver":
                    {
                        config.UseSqlServerStorage(dataAccessConfiguration.ConnectionString);
                        return config;
                    }
                case "sqllite":
                    {
                        config.UseSQLiteStorage(dataAccessConfiguration.ConnectionString);
                        return config;

                    }
                default:
                    return config;

            }
        }

        public static HangfireConfig GetHangfireConfiguration(
            this IConfiguration configuration)
        {
            var hangFireConfig = new HangfireConfig();
            configuration.Bind(nameof(HangfireConfig), hangFireConfig);
            return hangFireConfig;
        }
    }
}
