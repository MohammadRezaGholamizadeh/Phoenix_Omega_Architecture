using AccessControlLayer.AccessControll.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EFTech.AccessControls.Base
{
    public class EFUserResourceEntityMap :
        IEntityTypeConfiguration<UserResource>
    {
        public void Configure(EntityTypeBuilder<UserResource> _)
        {
            _.ToTable("ApplicationUsersResources");
            _.HasKey(_ => _.Id);
            _.Property(_ => _.UserId).IsRequired();
            _.Property(_ => _.RoleId).IsRequired(false);
            _.Property(_ => _.ResourceTypeId).IsRequired();
            _.Property(_ => _.ActionTypeId).IsRequired();

            _.HasOne(_ => _.User).WithMany().HasForeignKey(_ => _.UserId);
            _.HasMany(_ => _.TargetResourceId)
                .WithOne(_ => _.UserResource)
                .HasForeignKey(_ => _.UserResourceId);
        }
    }
}
