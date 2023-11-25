using AccessControlLayer.AccessControll.Domains;

namespace AccessControlLayer.Infrastructure.SeedDataInfra.Contracts
{
    public interface RoleActionSeedDataRepository
    {
        Task<string?> GetRoleIdByRoleName(string roleName);

        void AddRangeRoleActions(IEnumerable<RoleAction> roleAction);

        Task<bool> IsExistDuplicateRoleAction(
            string? roleId,
            List<string> actionTypesId);

        List<string> GetAllRoleActionThatNotRegistered(
                     List<string> actionTypesId,
                     string roleId);
        Task<List<RoleAction>> GetAllRoleActionByRoleId(string roleId);
        void RemoveAll(IEnumerable<RoleAction> deletingRoleAction);

        Task<List<GetApplicationUserInfo>>
            GetAllApplicationUserRoleByRoleId(string? roleId);

        void AddUserResources(
            List<UserResource> userresources);

        Task<List<UserResource>>
            FindAllUserResourceByRoleAndActionTypeId(
            IEnumerable<string> validActionForDelete,
            string roleId);

        void DeleteUserResources(
            List<UserResource> UserResources);

        long? GetLastSeedDataVersion();

        void SetVersionInfo(RoleActionSeedDataVersionInfo version);

        void DeleteVersionInfo(RoleActionSeedDataVersionInfo version);

        RoleActionSeedDataVersionInfo? FindSeedDataVersion(long? version);
        void Remove(RoleAction deletingRoleAction);
    }
}
