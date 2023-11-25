using ServiceLayer.Setups.ServicecInterfaces;

namespace ServiceLayer.Setups.TokenManagerInterface
{
    public interface UserTokenService : IService
    {
        string? UserId { get; }
        string? UserRole { get; }
        string TenantId { get; }
        bool IsInRole(string role);
        Dictionary<string, string> GetToken();
        Dictionary<string, string> GetQueryParams();
    }
}
