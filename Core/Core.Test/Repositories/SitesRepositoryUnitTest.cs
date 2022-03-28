using Core.Test.MockProvider;
using EntityFrameworkCoreMock.NSubstitute;
using HMSDigital.Core.Data;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Repositories
{
    public class SitesRepositoryUnitTest
    {
        private readonly ISieveProcessor _sieveProcessor;

        private readonly SieveModel _sieveModel;

        private readonly IUsersRepository _usersRepository;

        private readonly MockModels _mockModels;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DbContextMock<HMSDigitalAuditContext> _dbContextMock;

        private const string LOGGED_IN_USER = "LoggedInUser";

        public SitesRepositoryUnitTest()
        {
            var DummyOptions = new DbContextOptionsBuilder<HMSDigitalAuditContext>().UseSqlServer("FakeConnectionString").Options;
            var mockService = new MockServices();
            _httpContextAccessor = mockService.GetService<IHttpContextAccessor>();
            _mockModels = mockService.GetService<MockModels>();
            _sieveModel = new SieveModel();
            _usersRepository = mockService.GetService<IUsersRepository>();
            _sieveProcessor = mockService.GetService<ISieveProcessor>();
            _dbContextMock = new DbContextMock<HMSDigitalAuditContext>(DummyOptions, _httpContextAccessor);
        }

        [Fact]
        public async Task GetAllSitesShouldRetrivedAfterCachingOnFilterSieveFilter()
        {
            MockDBSet();

            //Cache sites on filters
            _sieveModel.Filters = "name@=*test site";
            var siteRepository = new SitesRepository(_dbContextMock.Object, _sieveProcessor);
            siteRepository.SieveModel = _sieveModel;
            var sitesResult = await siteRepository.GetAllAsync();
            Assert.NotEmpty(sitesResult);
            foreach (var site in sitesResult)
            {
                Assert.Contains("test site", site.Name);
            }

            //Get All sites
            siteRepository.SieveModel = new SieveModel();
            sitesResult = await siteRepository.GetAllAsync();
            Assert.NotEmpty(sitesResult);
            Assert.Equal(_mockModels.Sites.Count(), sitesResult.Count());
        }

        [Fact]
        public async Task SieveFilterShouldBeAppliedOnCacheSites()
        {
            MockDBSet(); 
            var siteRepository = new SitesRepository(_dbContextMock.Object, _sieveProcessor);
            //Get All sites
            siteRepository.SieveModel = new SieveModel();
            var sitesResult = await siteRepository.GetAllAsync();
            Assert.NotEmpty(sitesResult);
            Assert.Equal(_mockModels.Sites.Count(), sitesResult.Count());

            //filters on Cache sites
            _sieveModel.Filters = "name@=*test site";

            siteRepository.SieveModel = _sieveModel;
            sitesResult = await siteRepository.GetAllAsync();
            Assert.NotEmpty(sitesResult);
            foreach (var site in sitesResult)
            {
                Assert.Contains("test site", site.Name);
            }
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

        private void MockDBSet()
        {
            var sitesDbSetMock = _dbContextMock.CreateDbSetMock(x => x.Sites, _mockModels.Sites);

            //Add all sites permission
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 1);
            var userDbSetMock = _dbContextMock.CreateDbSetMock(x => x.Users, _mockModels.Users);
            var userRoles = _mockModels.Users.SelectMany(u => u.UserRoles).ToList();
            var userRoleDbSetMock = _dbContextMock.CreateDbSetMock(x => x.UserRoles, userRoles);
        }
    }
}
