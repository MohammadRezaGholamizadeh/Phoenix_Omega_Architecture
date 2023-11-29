using FluentMigrator;

namespace MigrationLayer.Migrations.BusinessTables
{
    [Migration(202311250021)]
    public class _202311250021_AddOrganizationTables : Migration
    {
        public override void Down()
        {
            Delete.Table("Organizations");
        }

        public override void Up()
        {
            Create.Table("Organizations")
                .WithColumn("Id").AsString(450).NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(450).NotNullable()
                .WithColumn("MobileNumber").AsString(13).NotNullable();
        }
    }
}
