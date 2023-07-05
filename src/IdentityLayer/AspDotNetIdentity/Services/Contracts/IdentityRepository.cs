using IdentityLayer.AspDotNetIdentity.Domain;
using ServiceLayer.Setups.RepositoryInterface;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts
{
    public interface IdentityRepository : IRepository
    {
        Task<ApplicationUser?> FindUserByUserNameOrEmail(string username);
        Task<List<ApplicationUser>> GetAllUser();
    }
}
