using AccessControlLayer.AccessControll.Actions.Base;
using AccessControlLayer.AccessControll.Contracts;
using AccessControlLayer.Infrastructure.MiddleWares;
using Microsoft.AspNetCore.Builder;

namespace AccessControlLayer.Infrastructure
{
    public static class ExtensionMethod
    {
        public static HashSet<Type> WithActionType<T>(
            this HashSet<Type> dependentActionList)
            where T : AccessAttribute
        {
            dependentActionList.Add(typeof(T));
            return dependentActionList;
        }

        public static HashSet<Type> WithActionTypes(
         this HashSet<Type> dependentActionList,
         params Type[] actions)
        {
            foreach (var item in actions)
            {
                dependentActionList.Add(item);
            }

            return dependentActionList;
        }

        public static void AddAccessControlMiddleWare(
            this IApplicationBuilder app)
        {
            app.UseMiddleware<AccessControlMiddleWare>();
        }

        public static async Task GenerateAccess(
            this Task task,
            AccessControlService accessControlService,
            object targetId)
        {
            await accessControlService
                  .GenerateAccessForUser(targetId);
        }
    }
}
