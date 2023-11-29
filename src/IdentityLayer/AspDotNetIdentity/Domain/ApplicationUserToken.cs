using DomainLayer.Entities.Organizations;
using Microsoft.AspNetCore.Identity;

namespace IdentityLayer.AspDotNetIdentity.Domain
{
    public class ApplicationUserToken : IdentityUserToken<string>, ITenant
    {
        public string TenantId { get; set; }
    }
}