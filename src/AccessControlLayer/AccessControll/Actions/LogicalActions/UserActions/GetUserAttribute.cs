using AccessControlLayer.AccessControll.Actions.Base;
using AccessControlLayer.Infrastructure;
using IdentityLayer.AspDotNetIdentity.Domain;
using System.Runtime.InteropServices;

namespace AccessControlLayer.AccessControll.Actions.LogicalActions.UserActions
{
    [Guid("fa0a2811-d479-4a58-aae0-4272a4bdea42")]
    public class GetUserAttribute
    : AccessAttribute
    {
        public GetUserAttribute()
     : base(typeof(ApplicationUser))
        {
            DependentActions
                .WithActionType<GetAllUserAttribute>();
        }
    }
}
