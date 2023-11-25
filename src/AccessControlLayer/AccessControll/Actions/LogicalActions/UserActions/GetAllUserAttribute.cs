using AccessControlLayer.AccessControll.Actions.Base;
using IdentityLayer.AspDotNetIdentity.Domain;
using System.Runtime.InteropServices;

namespace AccessControlLayer.AccessControll.Actions.LogicalActions.UserActions
{
    [Guid("9b1fac04-dec8-4ea8-9f49-58cffebf9835")]
    public class GetAllUserAttribute
    : AccessAttribute
    {
        public GetAllUserAttribute()
        : base(typeof(ApplicationUser))
        {
        }
    }
}
