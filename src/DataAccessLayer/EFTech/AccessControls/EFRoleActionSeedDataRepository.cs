using AccessControlLayer.AccessControll.Domains;
using AccessControlLayer.Infrastructure.SeedDataInfra;
using AccessControlLayer.Infrastructure.SeedDataInfra.Contracts;
using DataAccessLayer.EFTech.EFDataContexts;
using IdentityLayer.AspDotNetIdentity.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EFTech.AccessControls
{
    public class EFRoleActionSeedDataRepository
        : RoleActionSeedDataRepository
    {
        private readonly EFDataContext _context;

        public EFRoleActionSeedDataRepository(EFDataContext context)
        {
            _context = context;
        }

        public void AddUserResources(
            List<UserResource> userResources)
        {
            _context.UserResources.AddRange(userResources);
        }

        public void AddRangeRoleActions(IEnumerable<RoleAction> roleAction)
        {
            _context.Set<RoleAction>().AddRange(roleAction);
        }

        public void DeleteUserResources(
            List<UserResource> UserResources)
        {
            _context.UserResources
                .RemoveRange(UserResources);
        }

        public void DeleteVersionInfo(RoleActionSeedDataVersionInfo version)
        {
            _context.Set<RoleActionSeedDataVersionInfo>().Remove(version);
        }

        public async Task<List<UserResource>>
            FindAllUserResourceByRoleAndActionTypeId(
            IEnumerable<string> validActionForDelete,
            string roleId)
        {
            return await
                _context.UserResources
                        .IgnoreQueryFilters()
                        .Where(_ => _.RoleId == roleId
                                 && validActionForDelete
                                    .Contains(_.ActionTypeId))
                        .ToListAsync();
        }

        public RoleActionSeedDataVersionInfo? FindSeedDataVersion(long? version)
        {
            return _context.Set<RoleActionSeedDataVersionInfo>()
                           .SingleOrDefault(_ => _.Version == version);
        }

        public async Task<List<GetApplicationUserInfo>>
            GetAllApplicationUserRoleByRoleId(
            string? roleId)
        {
            return await _context.Set<ApplicationUserRole>()
                .IgnoreQueryFilters()
                .Where(_ => _.RoleId == roleId)
                .Select(_ => new GetApplicationUserInfo()
                {
                    //TenantId = _.ApplicationUser.TenantId,
                    //UserId = _.ApplicationUser.UserId
                }).ToListAsync();
        }

        public async Task<List<RoleAction>>
            GetAllRoleActionByRoleId(string roleId)
        {
            return
                await _context.Set<RoleAction>()
                              .Where(_ => _.RoleId == roleId)
                              .ToListAsync();
        }

        public List<string> GetAllRoleActionThatNotRegistered(
            List<string> actionTypesId,
            string roleId)
        {
            var result = _context
                         .Set<RoleAction>()
                         .Where(_ => _.RoleId == roleId
                                  && actionTypesId.Contains(_.ActionTypeId))
                         .Select(_ => _.ActionTypeId);
            actionTypesId.RemoveAll(_ => result.Contains(_));
            return actionTypesId;
        }

        public long? GetLastSeedDataVersion()
        {
            var version = _context.Set<RoleActionSeedDataVersionInfo>()
                                   .OrderBy(_ => _.Version)
                                   .LastOrDefault();
            return version?.Version;
        }

        public async Task<string?> GetRoleIdByRoleName(string roleName)
        {
            var role =
                await _context
                      .Set<ApplicationRole>()
                      .FirstOrDefaultAsync(_ => _.Name == roleName);
            return role?.Id;
        }

        public async Task<bool> IsExistDuplicateRoleAction(
            string? roleId,
            List<string> actionTypesId)
        {
            return await _context.Set<RoleAction>()
                                 .AnyAsync(_ => _.RoleId == roleId
                                             && actionTypesId
                                                .Contains(_.ActionTypeId));
        }

        public void RemoveAll(IEnumerable<RoleAction> deletingRoleAction)
        {
            _context.Set<RoleAction>().RemoveRange(deletingRoleAction);
        }

        public void Remove(RoleAction deletingRoleAction)
        {
            _context.Set<RoleAction>().Remove(deletingRoleAction);
        }

        public void SetVersionInfo(RoleActionSeedDataVersionInfo version)
        {
            _context.Set<RoleActionSeedDataVersionInfo>().Add(version);
        }
    }
}
