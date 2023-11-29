using IdentityLayer.AspDotNetIdentity.Domain;
using ServiceLayer.Setups.RepositoryInterfaces;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public interface ApplicationUserRepository : IRepository
    {
        Task<Guid?> GetUserIdByNationalCode(string nationalCode);
        Task<ApplicationUser> FindUserByNationalCode(string nationalCode);
        Task<bool> IsNationalCodeRegistered(Guid userId, string nationalCode);
        Task<GetApplicationUserDto> GetUserById(Guid userId);
        Task<IList<GetApplicationUserDto>> GetAllUsers();
        Task<ApplicationUser?> FindUserById(Guid userId);
        Task<IList<ApplicationUser>> GetRegistredUsers(string nationalCode, string countryCallingCode, string mobileNumber);
        Task<bool> isExist(Guid userId);
        Task<IList<GetTeamApplicationUserDto>> GetAllUserInThisTeam(int teamId);
        Guid GetRoleId(string roleName);
        Task<bool> IsAuthorThisUser(int businessContextId, Guid loginUserId);
        Task<bool> IsUserLastActiveAuthorInAnyTeams(Guid UserId);
        bool IsExistAll(List<Guid> applicationUserId);
    }
}
