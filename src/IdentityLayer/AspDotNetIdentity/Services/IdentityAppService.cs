using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts;
using IdentityLayer.AspDotNetIdentity.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace IdentityLayer.AspDotNetIdentity.Services
{
    public class IdentityAppService : IdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityRepository _identityRepository;

        public IdentityAppService(
            UserManager<ApplicationUser> userManager,
            IdentityRepository identityRepository)
        {
            _userManager = userManager;
            _identityRepository = identityRepository;
        }

        public async Task<IdentityResult> InitializeUser(
            ApplicationUser applicationUser)
        {
            if (applicationUser.Mobile != null)
            {
                applicationUser.Mobile.MobileNumber =
                    applicationUser.Mobile.MobileNumber.TrimStart('0');
            }

            var result = await _userManager.CreateAsync(
                applicationUser,
                applicationUser.Mobile!.MobileNumber);

            GuardAgainstInitializeUser(result);

            return result;
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _identityRepository.GetAllUser();
        }

        private static void GuardAgainstInitializeUser(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new InitializingUserFailedException();
        }

    }
}
