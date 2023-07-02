using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts.Dtos;
using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers.Identities
{
    [Route("api/v{version:apiVersion}/Identities")]
    [ApiController]
    [ApiVersion("1.0")]
    public class IdentitiesController : ControllerBase
    {
        private readonly IdentityService _identityService;

        public IdentitiesController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<IdentityResult> InitializeUser(
            ApplicationUser applicationUser)
        {
            return await _identityService.InitializeUser(applicationUser);
        }
        [HttpGet("all")]
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _identityService.GetAllUsers();
        }
    }
}
