using DataAccessLayer.EFTech.EFDataContexts;
using IdentityLayer.AspDotNetIdentity.Domain;
using IdentityLayer.AspDotNetIdentity.Services.Contracts;
using IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EFTech.EFRepositories.Identities
{
    public class EFIdentityRepository : IdentityRepository
    {
        private readonly EFDataContext _context;
        private readonly DbSet<ApplicationUser> _applicationUsers;

        public EFIdentityRepository(EFDataContext context)
        {
            _context = context;
            _applicationUsers = _context.ApplicationUsers;
        }

        public async Task<ApplicationUser?> FindUserByUserNameOrEmail(string username)
        {
            username = username.ToLowerInvariant()
                               .Replace(" ", "");

            return await _applicationUsers
                         .Where(_ => (_.UserName.ToLower().Replace(" ", "") != null
                                      && _.UserName.ToLower().Replace(" ", "") == username)
                                  || (_.Email != null
                                      && _.Email.ToLower().Replace(" ", "") == username))
                         .SingleOrDefaultAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUser()
        {
            return await _applicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser?> FindUserByMobileNumber(string mobileNumber)
        {
            mobileNumber = mobileNumber.Replace(" ", "").ToLower();
            return await _applicationUsers
                         .SingleOrDefaultAsync(_ => _.Mobile!.MobileNumber == mobileNumber); ;
        }

        public async Task<string?> GetUserIdByMobileNumber(string mobileNumber)
        {
            return await _applicationUsers.Where(_ => _.Mobile.MobileNumber == mobileNumber)
                                          .Select(_ => _.Id)
                                          .SingleOrDefaultAsync();
        }

        public async Task<bool> IsMobileNumberRegistered(string userId, string mobileNumber)
        {
            return await _applicationUsers
                         .AnyAsync(_ => _.Id != userId
                                     && _.Mobile.MobileNumber == mobileNumber);
        }

        public async Task<GetApplicationUserDto?> GetUserById(string userId)
        {
            return await _applicationUsers.Where(_ => _.Id == userId)
                                          .Select(_ => new GetApplicationUserDto
                                          {
                                              Id = _.Id,
                                              UserName = _.UserName,
                                              MobileNumber = _.Mobile.MobileNumber,
                                              CountryCallingCode = _.Mobile.CountryCallingCode,
                                          })
                                         .SingleOrDefaultAsync();
        }

        public async Task<ApplicationUser?> FindUserById(string userId)
        {
            return await _applicationUsers.SingleOrDefaultAsync(_ => _.Id == userId);
        }

        public async Task<IList<ApplicationUser>> GetRegistredUsers(
            string countryCallingCode,
            string mobileNumber)
        {
            return await _applicationUsers
                         .Where(_ => _.Mobile.CountryCallingCode == countryCallingCode
                                  && _.Mobile.MobileNumber == mobileNumber)
                        .Select(_ => new ApplicationUser
                        {
                            UserName = _.UserName,
                            Mobile = _.Mobile,
                            CreationDate = _.CreationDate
                        })
                        .ToListAsync();
        }

        public async Task<IList<GetApplicationUserDto>> GetAllUsers()
        {
            return await (from user in _applicationUsers

                          select new GetApplicationUserDto()
                          {
                              Id = user.Id,
                              MobileNumber = user.Mobile.MobileNumber,
                              CountryCallingCode = user.Mobile.CountryCallingCode,
                              UserName = user.UserName
                          }).ToListAsync();

        }

        public Task<bool> isExist(string userId)
        {
            return _applicationUsers.AnyAsync(_ => _.Id == userId);
        }

        public string? GetRoleId(string roleName)
        {
            return _context.Roles
                           .FirstOrDefault(_ => _.NormalizedName
                                                == roleName.ToUpper())?.Id;
        }

        public bool IsExistAll(List<string> applicationUserId)
        {
            return !applicationUserId
                    .Except(_context.ApplicationUsers.Select(_ => _.Id))
                    .Any();
        }

    }
}
