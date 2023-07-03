using ApplicationLayer.ServiceInterface;
using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos;
using Microsoft.AspNetCore.Identity;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts
{
    public interface IdentityService : IService
    {
        Task<IdentityResult> InitializeUser(
            AddApplicationUserDto applicationUser);

        Task<List<ApplicationUser>> GetAllUsers();
        Task<GetLoginDto> LoginUser(LoginDto dto);
    }
}
