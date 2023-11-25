using DomainLayer.Entities.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EFTech.EFEntityMaps.Organizations
{
    public class EFOrganizationEntityMAp : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> _)
        {
            _.ToTable("Organizations");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();

            _.Property(_ => _.Name).IsRequired();
            _.Property(_ => _.MobileNumber).IsRequired();
        }
    }
}
