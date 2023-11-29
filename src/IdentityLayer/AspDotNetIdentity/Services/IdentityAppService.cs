using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts;
using IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos;
using IdentityLayer.AspDotNetIdentity.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Setups.RepositoryInterfaces;
using ServiceLayer.Setups.TokenManagerInterface;
using System.Security.Claims;
using static ServiceLayer.Setups.Validations.NationalCodeValidator;

namespace IdentityLayer.AspDotNetIdentity.Services
{
    public class IdentityAppService : IdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityRepository _identityRepository;
        private readonly TokenManager _tokenManager;
        private readonly UnitOfWork _unitOfWork;

        public IdentityAppService(
            UserManager<ApplicationUser> userManager,
            IdentityRepository identityRepository,
            TokenManager tokenManager,
            UnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _identityRepository = identityRepository;
            _tokenManager = tokenManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> InitializeUser(
            AddApplicationUserDto dto)
        {
            if (dto.Mobile != null)
            {
                dto.Mobile.MobileNumber =
                   dto.Mobile.MobileNumber.TrimStart('0');
            }

            var applicationUser =
                new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Mobile = new Mobile()
                    {
                        MobileNumber = dto.Mobile.MobileNumber,
                        CountryCallingCode = dto.Mobile.CountryCallingCode
                    },
                    Email = dto.Email,
                    UserName = dto.Mobile.MobileNumber
                };

            var result = await _userManager.CreateAsync(
                applicationUser,
                dto.Mobile!.MobileNumber);

            GuardAgainstInitializeUser(result);

            return result;
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _identityRepository.GetAllUser();
        }


        public async Task<GetLoginDto> LoginUser(LoginDto dto)
        {
            var user = await _identityRepository
                             .FindUserByUserNameOrEmail(dto.Username);
            GuardAgainstUserNotFound(user);
            VerifyHashedPassword(user!, dto.Password);
            GuardAgainstUserIsInactive(user!.LockoutEnabled);

            var refreshToken = _tokenManager.GenerateRefreshToken();
            dto.SetRefreshToken(
                refreshToken, _tokenManager.GetRefreshTokenExpiryTime());

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);


            return new GetLoginDto()
            {
                AccessToken = _tokenManager.Generate(userClaims, userRoles, user.Id),
                RefreshToken = refreshToken
            };
        }
        public async Task<IdentityResult> AddUserToAdminRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            GuardAgainstUserNotFound(user);

            var result = await _userManager.AddToRoleAsync(user, SystemRoles.Admin);
            GuardAgainstAddUserToAdminRoleFailed(result);

            return result;
        }
        public async Task<IdentityResult> RemoveUserFromAdminRole(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            GuardAgainstUserNotFound(user);

            var result =
                await _userManager
                      .RemoveFromRoleAsync(user, SystemRoles.Admin);
            GuardAgainstRemoveUserFromAdminRoleFailed(result);

            return result;
        }
        public async Task<GetApplicationUserDto> GetUserById(string userId)
        {
            return await _identityRepository.GetUserById(userId);
        }
        public async Task<ApplicationUser> FindByUsername(string username)
        {
            var user =
                await _userManager.FindByNameAsync(username);
            GaurdAgainstWrongUsernameOrPassword(user);

            return user;
        }
        public bool VerifyHashedPassword(
            ApplicationUser applicationUser,
            string dtoPassword)
        {
            var passwordVerifiedResult =
                _userManager.PasswordHasher
                            .VerifyHashedPassword(
                                applicationUser,
                                applicationUser.PasswordHash,
                                dtoPassword);

            GuardAgainstWrongUsernameOrPassword(passwordVerifiedResult);

            return true;
        }
        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GetClaimsAsync(applicationUser);
        }
        public async Task<IList<string>> GetRolesAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GetRolesAsync(applicationUser);
        }
        public async Task UpdateUser(string userId, UpdateApplicationUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            await GuardAgainstDuplicateRegisteration(userId, dto.NationalCode);
            GaurdAgainstRegisterWithMobileNumber(dto.MobileNumber);
            user.Mobile = new Mobile
            {
                CountryCallingCode = dto.CountryCallingCode,
                MobileNumber = dto.MobileNumber
            };

            await _userManager.UpdateAsync(user);

        }
        public async Task ToggleActivation(string id, ToggleActivationDto dto)
        {
            var user = await _identityRepository.FindUserById(id);
            GaurdAgainstUserNotFound(user);

            await SetLockoutEnable(user!, !dto.Active);

            await _unitOfWork.SaveAllChangesAsync();
        }
        public async Task ChangePassword(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            GaurdAgainstUserNotFound(user);

            var checkResult = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
            GaurdAgainstCurrentPasswordIsNotCorrect(checkResult);
            var changeResult = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            GaurdAgainstChangePasswordFailed(changeResult);
        }
        private async Task<IdentityResult> SetLockoutEnable(ApplicationUser user, bool lockout)
        {
            var result =
                await _userManager.SetLockoutEnabledAsync(user, lockout);
            GaurdAgainstLockOutUserFailed(result);

            return result;
        }

        private static void GaurdAgainstChangePasswordFailed(IdentityResult changeResult)
        {
            if (!changeResult.Succeeded)
                throw new ChangePasswordFailedException();
        }
        private static void GaurdAgainstCurrentPasswordIsNotCorrect(bool checkResult)
        {
            if (!checkResult)
                throw new CurrentPasswordIsNotCorrectException();
        }
        private static void GuardAgainstInitializeUser(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new InitializingUserFailedException();
        }
        private static void GaurdAgainstLockOutUserFailed(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new LockOutUserFailedException();
        }
        private void GaurdAgainstUserNotFound(ApplicationUser? user)
        {
            if (user == null)
                throw new UserNotFoundException();
        }
        private async Task GuardAgainstDuplicateRegisteration(string userId, string mobileNumber)
        {
            if (await _identityRepository.IsMobileNumberRegistered(userId, mobileNumber))
                throw new NationalCodeAlreadyRegisteredException();
        }
        private void GaurdAgainstRegisterWithMobileNumber(string mobileNumber)
        {
            if (new MobileNumberAttribute().IsValid(mobileNumber) == false)
                throw new InvalidMobileNumberException();
        }
        private static void GuardAgainstUserIsInactive(bool isLockoutEnabled)
        {
            if (isLockoutEnabled)
            {
                throw new UserIsInactiveException();
            }
        }
        private static void GuardAgainstUserNotFound(ApplicationUser? user)
        {
            if (user == null)
            {
                throw new UserNotFoundException();
            };
        }
        private static void GuardAgainstAddUserToAdminRoleFailed(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new AddUserToAdminRoleFailedException();
        }
        private static void GuardAgainstRemoveUserFromAdminRoleFailed(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new RemoveUserFromAdminRoleFailedException();
        }
        private static void GaurdAgainstWrongUsernameOrPassword(ApplicationUser user)
        {
            if (user == null)
                throw new WrongUsernameOrPasswordException();
        }
        private static void GuardAgainstWrongUsernameOrPassword(
             PasswordVerificationResult passwordVerifiedResult)
        {
            if (passwordVerifiedResult != PasswordVerificationResult.Success)
                throw new WrongUsernameOrPasswordException();
        }
    }
}
