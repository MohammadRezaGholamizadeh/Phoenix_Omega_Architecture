using DomainLayer.Entities.Organizations;

namespace IdentityLayer.AspDotNetIdentity.Domain
{
    public class ApplicationUserRefreshToken : ITenant
    {
        public ApplicationUserRefreshToken()
        {
        }

        public ApplicationUserRefreshToken(
            string userId,
            string token,
            DateTime refreshTokenExpiryTime)
        {
            UserId = userId;
            Token = token;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string Token { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}