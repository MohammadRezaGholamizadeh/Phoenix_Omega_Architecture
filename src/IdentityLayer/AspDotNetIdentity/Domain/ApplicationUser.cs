using DomainLayer.Entities.Organizations;
using Microsoft.AspNetCore.Identity;

namespace IdentityLayer.AspDotNetIdentity.Domain
{
    public class ApplicationUser : IdentityUser<string>, ITenant
    {
        public ApplicationUser(Mobile mobile, DateTime creationDate)
        {
            Mobile = mobile;
            CreationDate = creationDate;
        }
        public ApplicationUser()
        {

        }
        public string TenantId { get; set; }
        public Organization Organization { get; set; }
        public Mobile Mobile { get; set; }
        public DateTime CreationDate { get; set; }
        public ApplicationUserRefreshToken? RefreshToken { get; set; }
    }
}