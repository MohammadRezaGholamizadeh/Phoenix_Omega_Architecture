using AccessControlLayer.AccessControll.Actions.Base;
using AccessControlLayer.AccessControll.Domains;
using AccessControlLayer.Infrastructure;
using System.Runtime.InteropServices;

namespace AccessControlLayer.AccessControll.Actions.LogicalActions.AccessControls
{
    [Guid("6c822c2c-5875-4547-8269-c809e7f43963")]
    public class UpdateUserResourceAttribute : AccessAttribute
    {
        public UpdateUserResourceAttribute()
            : base(typeof(UserResource))
        {
            DependentActions
                .WithActionType<GetAllUserResourceAttribute>();
        }
    }
}
