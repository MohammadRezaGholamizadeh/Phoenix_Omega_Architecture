using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IMigrationRunner =
      InfrastructureLayer.MigrationLayerConfigurations
                         .Contracts.IMigrationRunner;

namespace MigrationLayer
{
    public class MigrationRunner : IMigrationRunner
    {
        private static IConfiguration _configuration;

        public MigrationRunner(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Initialize(string[]? args)
        {
            RunRootMigrations(args);
        }
        public static void RunRootMigrations(string[]? args)
        {
            var options = GetSettings(args, _configuration);

            CreateDatabaseSchema(options.ConnectionString);

            var runner = CreateRunner(options.ConnectionString, options);
            runner.MigrateUp();
        }

        public static void CreateDatabaseSchema(string connectionString)
        {
            var databaseName = GetDatabaseName(connectionString);
            string masterConnectionString = ChangeDatabaseName(
                connectionString, "master");
            var commandScript =
                $"if db_id(N'{databaseName}') is null create database" +
                $" [{databaseName}]";

            using var connection = new SqlConnection(masterConnectionString);
            using var command = new SqlCommand(commandScript, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        private static string ChangeDatabaseName(
            string connectionString, string databaseName)
        {
            var csb = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName
            };
            return csb.ConnectionString;
        }

        private static string GetDatabaseName(string connectionString)
        {
            return new SqlConnectionStringBuilder(
                connectionString).InitialCatalog;
        }

        private static FluentMigrator.Runner.IMigrationRunner CreateRunner(
            string connectionString, MigrationSettings options)
        {
            var container = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(_ => _
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(MigrationRunner).Assembly).For.All())
                .AddSingleton<MigrationSettings>(options)
                .AddSingleton<ScriptResourceManager>()
                .AddLogging(_ => _.AddFluentMigratorConsole())
                .BuildServiceProvider();
            return container.GetRequiredService<FluentMigrator.Runner.IMigrationRunner>();
        }

        private static MigrationSettings GetSettings(
            string[]? args, IConfiguration configuration)
        {
            var settings = new MigrationSettings();
            settings.ConnectionString =
                configuration
                .GetSection("DataAccessConfig")
                .GetValue<string>("ConnectionString");

            return settings;
        }
    }

    // setting
    public class MigrationSettings
    {
        public string ConnectionString { get; set; }
    }
}
