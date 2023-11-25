using AccessControlLayer.AccessControll.Actions.Base;
using AccessControlLayer.AccessControll.Domains;

namespace AccessControlLayer.AccessControll.Actions.LogicalActions.AccessControls
{
    public class GetAllUserResourceAttribute : AccessAttribute
    {
        public GetAllUserResourceAttribute()
            : base(typeof(UserResource))
        {
        }
    }
}
