using IdentityLayer.AspDotNetIdentity.Domain;
using Invio.Extensions.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InfrastructureLayer.IdentityConfigurations.AspIdentities
{
    public static class AspDotnetIdentityConfiguration
    {
        public static IServiceCollection AddAspIdentity(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var aspIdentityConfig =
                configuration.GetAspIdentityConfig();
            var key = Encoding.ASCII.GetBytes(aspIdentityConfig.JwtBearerTokenConfig.SecretKey);
            services.AddIdentity<ApplicationUser, ApplicationRole>(
                setup =>
                {
                    setup.Password.RequireNonAlphanumeric =
                            aspIdentityConfig.PasswordConfig
                                             .RequireNonAlphanumeric;

                    setup.Password.RequireLowercase =
                            aspIdentityConfig.PasswordConfig
                                             .RequireLowercase;

                    setup.Password.RequireUppercase =
                            aspIdentityConfig.PasswordConfig
                                             .RequireUppercase;

                    setup.Password.RequiredLength =
                            aspIdentityConfig.PasswordConfig
                                             .RequiredLength;

                    setup.Password.RequireDigit =
                            aspIdentityConfig.PasswordConfig
                                             .RequireDigit;

                    setup.Lockout.AllowedForNewUsers =
                            aspIdentityConfig.LockoutAllowedForNewUsers;
                })
                ;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = aspIdentityConfig.JwtBearerTokenConfig.RequireHttpsMetadata;
                options.SaveToken = aspIdentityConfig.JwtBearerTokenConfig.SaveToken;
                options.Audience = aspIdentityConfig.JwtBearerTokenConfig.Audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = aspIdentityConfig.JwtBearerTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.AddQueryStringAuthentication();
            }); ;

            return services;
        }


        public static AspIdentityConfig GetAspIdentityConfig(
           this IConfiguration configuration)
        {
            var aspIdentityConfig = new AspIdentityConfig();
            configuration.Bind(nameof(AspIdentityConfig), aspIdentityConfig);
            return aspIdentityConfig;
        }
    }

    public class AspIdentityConfig
    {
        public PasswordConfig PasswordConfig { get; set; }
        public JwtBearerTokenConfig JwtBearerTokenConfig { get; set; }
        public bool LockoutAllowedForNewUsers { get; set; }

    }

    public class PasswordConfig
    {
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public int RequiredLength { get; set; }
        public bool RequireDigit { get; set; }
    }

    public class JwtBearerTokenConfig
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiryTimeInSeconds { get; set; }
        public int RefreshTokenExpiryTimeInSeconds { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public bool SaveToken { get; set; }
    }
}
