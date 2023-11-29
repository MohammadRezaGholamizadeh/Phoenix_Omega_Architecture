using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Setups.ServicecInterfaces;
using System.Security.Claims;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts
{
    public interface IdentityService : IService
    {
        Task<IdentityResult> InitializeUser(
            AddApplicationUserDto applicationUser);

        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> FindByUsername(string username);
        Task<GetApplicationUserDto> GetUserById(string id);
        bool VerifyHashedPassword(ApplicationUser applicationUser, string dtoPassword);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser applicationUser);
        Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser);
        Task UpdateUser(string userId, UpdateApplicationUserDto appUserUpdateDto);
        Task ToggleActivation(string id, ToggleActivationDto dto);
        Task ChangePassword(string id, ChangePasswordDto dto);
        Task<IdentityResult> AddUserToAdminRole(string userId);
        Task<IdentityResult> RemoveUserFromAdminRole(string userId);
        Task<GetLoginDto> LoginUser(LoginDto dto);
    }
}
