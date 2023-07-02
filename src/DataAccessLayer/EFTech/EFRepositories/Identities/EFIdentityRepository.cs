using DataAccessLayer.EFTech.EFDataContexts;
using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EFTech.EFRepositories.Identities
{
    public class EFIdentityRepository : IdentityRepository
    {
        private readonly EFDataContext _context;

        public EFIdentityRepository(EFDataContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetAllUser()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }
    }
}
