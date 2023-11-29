using DomainLayer.Entities.Organizations;
using Microsoft.AspNetCore.Identity;

namespace IdentityLayer.AspDotNetIdentity.Domain
{
    public class ApplicationUserClaim : IdentityUserClaim<string>, ITenant
    {
        public string TenantId { get; set; }
    }
}