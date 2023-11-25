using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AccessControlLayer.Infrastructure
{
    public interface UserTokenService
    {
        string UserId { get; }
        IList<string> Roles { get; }
        string UserName { get; }
        bool Admin { get; }
        bool Driver { get; }
        string TenantId { get; }
    }

    public class UserTokenAppService : UserTokenService
    {
        private readonly IHttpContextAccessor accessor;

        public UserTokenAppService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public string UserId => GetUserIdFromJwtToken();
        public bool Admin => Roles.Any(_ => _ == "YoozAdmin");
        public bool Driver => Roles.Any(_ => _ == "YoozDriver");
        public string UserName => GetUserNameFromJwtToken();
        public string TenantId => GetTenantIdFromHeader();

        private string GetTenantIdFromHeader()
        {
            if (accessor.HttpContext != null)
            {
                return accessor
                       .HttpContext
                       .Request
                       .Headers
                       .SingleOrDefault(_ => _.Key.ToLower()
                                          == "tenantid").Value;
            }
            else
            {
                return default;
            }
        }

        public IList<string> Roles
        {
            get
            {
                return accessor.HttpContext
                    .User
                    .Claims
                    .Where(_ => _.Type == ClaimTypes.Role)
                    .Select(_ => _.Value)
                    .ToList();
            }
        }

        private string? GetUserNameFromJwtToken()
        {
            return accessor.HttpContext?
                .User
                .Claims
                .FirstOrDefault(_ => _.Type == "preferred_username")
                ?.Value;
        }

        private string? GetUserIdFromJwtToken()
        {
            return accessor.HttpContext?
                .User.Claims
                .FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }
    }
}
