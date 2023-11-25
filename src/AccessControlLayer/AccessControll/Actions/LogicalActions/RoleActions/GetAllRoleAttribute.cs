using AccessControlLayer.AccessControll.Actions.Base;
using IdentityLayer.AspDotNetIdentity.Domain;
using System.Runtime.InteropServices;

namespace AccessControlLayer.AccessControll.Actions.LogicalActions.RoleActions
{
    [Guid("d7065886-74ff-45c1-97f3-9c1472f39333")]
    public class GetAllRoleAttribute
    : AccessAttribute
    {
        public GetAllRoleAttribute()
        : base(typeof(ApplicationRole))
        {
        }
    }
}
