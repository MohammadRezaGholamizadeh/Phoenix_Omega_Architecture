using DomainLayer.Entities.Organizations;
using IdentityLayer.AspDotNetIdentity.Domain;
using System.Runtime.InteropServices;

namespace AccessControlLayer.AccessControll.Domains
{
    [Guid("b6bf6d2d-7fc2-47f7-8b06-58f2f001d4a5")]
    public class UserResource : ITenant
    {
        public UserResource()
        {
            TargetResourceId = new List<ResourceId>();
        }
        public string TenantId { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? RoleId { get; set; }
        public string ResourceTypeId { get; set; }
        public string ActionTypeId { get; set; }
        public List<ResourceId> TargetResourceId { get; set; }
    }
}
