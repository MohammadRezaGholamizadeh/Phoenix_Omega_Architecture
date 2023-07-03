﻿using ApplicationLayer.ServiceInterface;

namespace InfrastructureLayer.IdentityConfigurations.TokensManager
{
    public interface UserTokenService : IService
    {
        string? UserId { get; }
        string? UserRole { get; }
        bool IsInRole(string role);
        Dictionary<string, string> GetToken();
        Dictionary<string, string> GetQueryParams();
    }
}
