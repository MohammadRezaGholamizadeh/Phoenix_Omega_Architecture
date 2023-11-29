using DataAccessLayer.EFTech.EFDataContexts;
using IdentityLayer.AspDotNetIdentity.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace UserStoriesManagement.RestApi.Config
{
    public class AspIdentitySeedData
    {
        public IServiceProvider _serviceProvider;
        public AspIdentitySeedData(IServiceProvider service)
        {
            _serviceProvider = service;
        }

        public async Task Initialize()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = _serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var context = _serviceProvider.GetRequiredService<EFDataContext>();

                if (!context.Roles.Any())
                {
                    var roles = new List<string>()
                    {
                        SystemRoles.Admin
                    };

                    foreach (var role in roles)
                    {
                        var applicationRole = new ApplicationRole { Name = role, NormalizedName = role.ToUpper() , Id = Guid.NewGuid().ToString() };
                        await roleManager.CreateAsync(applicationRole);
                    }
                }

                var admin = GenerateAdmin();

                if (!context.Set<ApplicationUser>().Any())
                {
                    var result = await userManager.CreateAsync(admin, "1234567890");


                    await userManager.AddToRoleAsync(admin, SystemRoles.Admin);
                }
            }
        }

        private ApplicationUser GenerateAdmin()
        {
            return new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now,
                UserName = "1234567890",
                Mobile = new Mobile() { MobileNumber = "9384834683", CountryCallingCode = "+98" },
            };
        }

    }
}
