using IdentityLayer.AspDotNetIdentity.Domain;
using ServiceLayer.RepositoryInterface;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts
{
    public interface IdentityRepository : IRepository
    {
        Task<List<ApplicationUser>> GetAllUser();
    }
}
