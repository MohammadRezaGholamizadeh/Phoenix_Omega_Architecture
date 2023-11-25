using AccessControlLayer.AccessControll.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EFTech.AccessControls
{
    public class EFRoleActionVersionInfoEntityMap
        : IEntityTypeConfiguration<RoleActionSeedDataVersionInfo>
    {
        public void Configure(
            EntityTypeBuilder<RoleActionSeedDataVersionInfo> _)
        {
            _.ToTable("RoleActionVersionsInfo");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Version).IsRequired();
        }
    }
}
