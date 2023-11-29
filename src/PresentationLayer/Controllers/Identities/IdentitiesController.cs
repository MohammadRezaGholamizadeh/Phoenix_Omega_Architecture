using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts;
using IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos;
using InfrastructureLayer.IdentityConfigurations.AspIdentities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Setups.TokenManagerInterface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PresentationLayer.Controllers.Identities
{
    [Route("api/v{version:apiVersion}/Identities")]
    [ApiController]
    [ApiVersion("1.0")]
    public class IdentitiesController : ControllerBase
    {
        private readonly IdentityService _identityService;
        private readonly UserTokenService _userTokenService;
        private readonly IOptions<JwtBearerTokenSetting> _jwtTokenOptions;

        public IdentitiesController(
            IdentityService identityService,
            UserTokenService userTokenService,
            IOptions<JwtBearerTokenSetting> jwtTokenOptions)
        {
            _identityService = identityService;
            _userTokenService = userTokenService;
            _jwtTokenOptions = jwtTokenOptions;
        }

        [HttpPost]
        public async Task<IdentityResult> InitializeUser(
            AddApplicationUserDto dto)
        {
            return await _identityService.InitializeUser(dto);
        }

        [HttpGet("all")]
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _identityService.GetAllUsers();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginDto dto)
        {
            var applicationUser = await ValidateApplicationUser(dto);

            return Ok(await GenerateToken(applicationUser));
        }

        [HttpPut("{userId}")]
        public async Task EditUser(string userId, UpdateApplicationUserDto dto)
        {
            await _identityService.UpdateUser(userId, dto);
        }

        [HttpPatch("changing-password")]
        public async Task ChangePassword(ChangePasswordDto dto)
        {
            await _identityService.ChangePassword(_userTokenService.UserId, dto);
        }
        [HttpPatch("{userId}/set-to-admin-role")]
        public async Task SetUserToRole(string userId)
        {
            await _identityService.AddUserToAdminRole(userId);
        }

        [HttpPatch("{userId}/remove-admin-role")]
        public async Task RemoveUserFromRole(string userId)
        {
            await _identityService.RemoveUserFromAdminRole(userId);
        }
        [HttpGet("user-profile")]
        [Authorize]
        public async Task<GetApplicationUserDto> GetUserProfile()
        {
            return await _identityService.GetUserById(_userTokenService.UserId);
        }
        private async Task<ApplicationUser> ValidateApplicationUser(LoginDto dto)
        {
            var applicationUser = await _identityService.FindByUsername(dto.Username);
            _identityService.VerifyHashedPassword(applicationUser, dto.Password);
            return applicationUser;
        }

        private async Task<string> GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenOptions.Value.SecretKey);

            var userClaims = await _identityService.GetClaimsAsync(applicationUser);
            var userRoles = await _identityService.GetRolesAsync(applicationUser);

            var tokenClaims = new ClaimsIdentity();
            tokenClaims.AddClaim(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));

            WriteUserRolesToTokenClaims(ref tokenClaims, userRoles);
            WriteUserClaimsToTokenClaims(ref tokenClaims, userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = tokenClaims,

                Expires = DateTime.UtcNow.AddSeconds(_jwtTokenOptions.Value.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtTokenOptions.Value.Audience,
                Issuer = _jwtTokenOptions.Value.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void WriteUserClaimsToTokenClaims(ref ClaimsIdentity tokenClaims, IList<Claim> userClaims)
        {
            foreach (var claim in userClaims)
            {
                tokenClaims.AddClaim(new Claim(claim.Type, claim.Value));
            }
        }

        private void WriteUserRolesToTokenClaims(ref ClaimsIdentity tokenClaims, IList<string> userRoles)
        {
            foreach (var role in userRoles)
            {
                tokenClaims.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

    }
}
