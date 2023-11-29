using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos;
using ServiceLayer.Setups.RepositoryInterfaces;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts
{
    public interface IdentityRepository : IRepository
    {
        Task<ApplicationUser?> FindUserByUserNameOrEmail(string username);
        Task<List<ApplicationUser>> GetAllUser();
        Task<string?> GetUserIdByMobileNumber(string mobileNumber);
        Task<ApplicationUser?> FindUserByMobileNumber(string mobileNumber);
        Task<bool> IsMobileNumberRegistered(string userId, string mobileNumber);
        Task<GetApplicationUserDto?> GetUserById(string userId);
        Task<IList<GetApplicationUserDto>> GetAllUsers();
        Task<ApplicationUser?> FindUserById(string userId);
        Task<IList<ApplicationUser>> GetRegistredUsers(
            string countryCallingCode, 
            string mobileNumber);
        Task<bool> isExist(string userId);
        string? GetRoleId(string roleName);
        bool IsExistAll(List<string> applicationUserId);

    }
}
