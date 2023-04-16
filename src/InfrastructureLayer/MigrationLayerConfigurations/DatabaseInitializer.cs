using InfrastructureLayer.MigrationLayerConfigurations.Contracts;

namespace InfrastructureLayer.MigrationLayerConfigurations
{
    public class DatabaseInitializer
    {
        private static IMigrationRunner _databaseInitializer;

        public DatabaseInitializer(
            IMigrationRunner databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }

        public void Initialize(string[]? args)
        {
            _databaseInitializer.Initialize(args);
        }
    }
}
