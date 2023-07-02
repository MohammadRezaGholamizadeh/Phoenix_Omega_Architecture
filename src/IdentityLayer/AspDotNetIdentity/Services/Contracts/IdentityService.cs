using ApplicationLayer.ServiceInterface;
using IdentityLayer.AspDotNetIdentity.Domain;
using Microsoft.AspNetCore.Identity;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts
{
    public interface IdentityService : IService
    {
        Task<IdentityResult> InitializeUser(
            ApplicationUser applicationUser);

        Task<List<ApplicationUser>> GetAllUsers();
    }
}
