using IdentityLayer.AspDotNetIdentity.Domain;

namespace AccessControlLayer.AccessControll.Domains
{
    public class RoleAction
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public string ActionTypeId { get; set; }
    }
}
