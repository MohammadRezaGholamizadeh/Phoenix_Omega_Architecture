using AccessControlLayer.AccessControll.Contracts.Dto;

namespace AccessControlLayer.AccessControll.Contracts
{
    public interface AccessControlService
    {
        Task HasAccessOnResource(
             object? resourceId = null);

        List<GetActionTypeDto>? GetAllActionTypes();

        Task AddUserResource(
             string userId,
             AddUserResourceAccessDto access);
        Task GenerateAccessForUser(object targetId);
        Task DeleteUserResource(string userId, DeleteUserResourceDto dto);

        Task<List<GetApplicationUserReource>>
            GetAllUserResourceByUserId(string userId);

        Task<List<GetUserAccessResultDto>>
            GetAllUserAccessResultByActionTypeId(
            AllActionTypeIdDto allActionTypeIdDto);

        Task GenerateAccessForApplicationUserRole(
            List<string> roleIds,
            string userId);
        Task GenerateAccessForApplicationUserRoleByTav(
            List<string> roleIds,
            string userId,
            string tenantId);

        Task DeleteAccessForApplicationUserRole(
            List<string> deletedRoles,
            string userId);
    }
}
