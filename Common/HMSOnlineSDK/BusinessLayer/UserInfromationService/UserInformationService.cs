using HMSOnlineSDK.BusinessLayer.Interfaces;
using HMSOnlineSDK.Data.Models;
using HMSOnlineSDK.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSOnlineSDK.BusinessLayer
{
    public class UserInformationService : IUserInformationService
    {
        private readonly IUserInformationRepository _userInformationRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IUserSitesRepository _userSitesRepository;

        public UserInformationService(IUserInformationRepository userInformationRepository,
            ISitesRepository sitesRepository,
            IUserSitesRepository userSitesRepository)
        {
            _userInformationRepository = userInformationRepository;
            _sitesRepository = sitesRepository;
            _userSitesRepository = userSitesRepository;
        }

        public async Task<UserInformations> CreateUser(UserInformations userInformations)
        {
            var existingUser = await _userInformationRepository.GetAsync(u => u.Username == userInformations.Username);
            if (existingUser != null)
            {
                return existingUser;
            }
            var site = await _sitesRepository.GetByIdAsync(userInformations.DefaultSiteId ?? 0);
            if (site == null)
            {
                throw new ValidationException($"SiteId ({userInformations.DefaultSiteId}) is not valid");
            }
            var user = new UserInformations()
            {
                Username = userInformations.Username,
                DefaultSiteId = userInformations.DefaultSiteId,
                UserSites = new List<UserSites>()
                {
                    new UserSites()
                    {
                        SiteId = userInformations.DefaultSiteId??0
                    }
                }
            };
            await _userInformationRepository.AddAsync(user);
            return user;
        }

        public async Task<UserInformations> UpdateSites(string userName, int defaultSiteId, IEnumerable<int> siteIds, string userId)
        {
            if (!siteIds.Contains(defaultSiteId))
            {
                throw new ValidationException($"default siteId ({defaultSiteId}) should be present in siteIds");
            }
            var sites = await _sitesRepository.GetManyAsync(s => siteIds.Contains(s.Id));
            var invalidSiteIds = siteIds.Except(sites.Select(s => s.Id));
            if (invalidSiteIds.Count() > 0)
            {
                throw new ValidationException($"SiteIds ({string.Join(",", invalidSiteIds)}) are not valid");
            }

            var user = await _userInformationRepository.GetAsync(u => u.Username == userName);
            if (user == null)
            {
                user = new UserInformations()
                {
                    Username = userName,
                    DefaultSiteId = defaultSiteId,
                    UserSites = new List<UserSites>()
                    {
                        new UserSites()
                        {
                            SiteId = defaultSiteId
                        }
                    }
                };
                await _userInformationRepository.AddAsync(user);
            }

            var usersExixtingSiteIds = user.UserSites.Select(us => us.Id).ToList();
            if (usersExixtingSiteIds.Count() > 0)
            {
                user.UserSites.Clear();
                await _userSitesRepository.DeleteAsync(us => usersExixtingSiteIds.Contains(us.Id));
            }

            user.DefaultSiteId = defaultSiteId;

            foreach (var site in sites)
            {
                user.UserSites.Add(new UserSites()
                {
                    SiteId = site.Id
                });
            }

            await _userInformationRepository.UpdateAsync(user);
            return user;
        }

        public async Task<UserInformations> UpdateDriver(string userName, int driverId)
        {
            var user = await _userInformationRepository.GetAsync(u => u.Username == userName);
            if (user == null)
            {
                throw new ValidationException($"User with username ({userName}) not found");
            }
            user.DriverId = driverId;
            await _userInformationRepository.UpdateAsync(user);
            return user;
        }

        public async Task<(int?, IEnumerable<Sites>)> GetUserSites(string userName)
        {
            var user = await _userInformationRepository.GetAsync(u => u.Username == userName);
            if (user == null)
            {
                return (null, new Sites[] { });
            }
            return (user.DefaultSiteId, user.UserSites.Select(u => u.Site));
        }
    }
}
