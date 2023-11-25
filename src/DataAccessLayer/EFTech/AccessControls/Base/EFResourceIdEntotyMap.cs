using AccessControlLayer.AccessControll.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EFTech.AccessControls.Base
{
    public class EFResourceIdEntotyMap
        : IEntityTypeConfiguration<ResourceId>
    {
        public void Configure(EntityTypeBuilder<ResourceId> _)
        {
            _.ToTable("ResourceIds");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.UserResourceId).IsRequired();
            _.Property(_ => _.TargetResourceId).IsRequired();

            _.HasOne(_ => _.UserResource)
                .WithMany(_ => _.TargetResourceId)
                .HasForeignKey(_ => _.UserResourceId);
        }
    }
}
