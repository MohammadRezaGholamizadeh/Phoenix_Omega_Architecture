namespace AccessControlLayer.Infrastructure.SeedDataInfra
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SeedDataVersionAttribute : Attribute
    {
        public SeedDataVersionAttribute(long version)
        {
            Version = version;
        }

        public long Version { get; set; }
    }
}
