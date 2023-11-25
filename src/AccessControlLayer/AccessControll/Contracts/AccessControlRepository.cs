using AccessControlLayer.AccessControll.Contracts.Dto;
using AccessControlLayer.AccessControll.Domains;

namespace AccessControlLayer.AccessControll.Contracts
{
    public interface AccessControlRepository
    {
        bool CheckUserAccess(
             string oauthUserId,
             AccessActionDto mainAction,
             AccessActionDto[] dependentActions,
             string? resourceId);

        Task<string?> GetApplicationUserIdByUserId(string userId);

        Task AddRange(IEnumerable<UserResource> resources);

        Task<bool> IsDuplicateActionForUser(
            string userId,
            IEnumerable<string> actionTypesId);

        Task<bool> IsExistApplicationUserByUserId(string userId);

        Task<List<UserResource>>
            FindAllDeletingUserResources(
            string userId,
            List<string> deleteingUserResourcesId);

        void DeleteAll(
            IEnumerable<UserResource> deletingUserResource);

        Task<List<GetApplicationUserReource>>
            GetAllUserResourceByUserId(string userId);

        Task<List<UserResource>>
            GetAllUserResourceByUserIdAndActionTypeId(
            IEnumerable<string> assignableActions,
            string userId);

        Task<List<GetUserAccessResultDto>>
            GetAllUserAccessResultByActionTypeId(
            string oauthUserId,
            List<string> actionTypesId);
        Task<bool> IsAllRoleExist(List<string> roleIds);
        Task<bool> IsExistUserByUserId(string userId);
        Task<List<GetRoleActionDto>> GetAllRoleActionByRoleId(
            List<string> roleIds);
        Task<List<UserResource>> GetAllUserResourceByRolesId(
            List<string> rolesId);

        Task<List<UserResource>>
            GetAllUserResourceByRolesIdAndUserId(
            List<string> rolesId,
            string userId);
    }
}
