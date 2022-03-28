using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class SitesServiceUnitTest
    {
        private readonly SieveModel _sieveModel;

        private readonly ISitesService _sitesService;

        private readonly IUsersRepository _usersRepository;

        private const string LOGGED_IN_USER = "LoggedInUser";

        public SitesServiceUnitTest()
        {
            var mockService = new MockServices();
            _sitesService = mockService.GetService<ISitesService>();
            _sieveModel = new SieveModel();
            _usersRepository = mockService.GetService<IUsersRepository>();
        }

        [Fact]
        public async Task GetSitesShouldReturnValidList()
        {
            var sitesResult = await _sitesService.GetAllSites(_sieveModel);
            Assert.NotEmpty(sitesResult.Records);
        }

        [Fact]
        public async Task ShouldGetSiteForValidSiteId()
        {
            var siteResult = await _sitesService.GetSiteById(1);
            Assert.NotNull(siteResult);
        }

        [Fact]
        public async Task ShouldNotGetSiteForInValidSiteId()
        {
            var siteResult = await _sitesService.GetSiteById(125);
            Assert.Null(siteResult);
        }

        [Fact]
        public async Task ShouldGetSiteToHigherAccessUserAfterLowerAccessUser()
        {

            var driverSiteId = 1;
            var nonDriverSiteId = 100;
            // Add driver role for loggedin user
            AddUserRoleToUser(LOGGED_IN_USER, driverSiteId.ToString(), ResourceTypes.Site.ToString(), 1);

            var accessibleSite = await _sitesService.GetSiteById(driverSiteId);
            Assert.NotNull(accessibleSite);
            Assert.Equal(driverSiteId, accessibleSite.Id);

            var nonAccessibleSite = await _sitesService.GetSiteById(nonDriverSiteId);
            Assert.Null(nonAccessibleSite);

            // Add all sites permission 
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 1);

            var site = await _sitesService.GetSiteById(nonDriverSiteId);
            Assert.NotNull(site);
            Assert.Equal(nonDriverSiteId, site.Id);
        }

        [Fact]
        public async Task ShouldNotGetSiteToLowerAccessUserAfterHigherAccessUser()
        {
            var nonDriverSiteId = 100;

            // Add all sites permission 
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 1);

            var site = await _sitesService.GetSiteById(nonDriverSiteId);
            Assert.NotNull(site);
            Assert.Equal(nonDriverSiteId, site.Id);

            // Add driver role for loggedin user
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 1);

            var nonAccessibleSite = await _sitesService.GetSiteById(nonDriverSiteId);
            Assert.Null(nonAccessibleSite);

        }

        private void AddUserRoleToUser(string cognitoUserId, string resourceId, string resourceType, int roleId)
        {
            var user = _usersRepository.GetByCognitoUserId(cognitoUserId).Result;
            user.UserRoles = new List<UserRoles>()
            {
                new UserRoles()
                {
                    Id = new Random().Next(10000, 100000),
                    ResourceId = resourceId,
                    ResourceType = resourceType,
                    RoleId = roleId,
                    UserId = user.Id
                }
            };
            _usersRepository.UpdateAsync(user);
        }
    }
}
