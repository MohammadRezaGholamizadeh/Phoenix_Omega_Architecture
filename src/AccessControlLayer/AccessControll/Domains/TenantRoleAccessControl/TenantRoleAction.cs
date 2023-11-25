using System.Runtime.InteropServices;

namespace AccessControlLayer.AccessControll.Domains.TenantRoleAccessControl
{
    [Guid("2d363d6c-1655-4a28-be4e-63107eaf0308")]
    public class TenantRoleAction
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string Action { get; set; }
    }
}
