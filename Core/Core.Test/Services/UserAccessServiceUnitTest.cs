using Core.Test.MockProvider;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using HMSDigital.Core.Data.Repositories.Interfaces;

namespace Core.Test.Services
{
    public class UserAccessServiceUnitTest
    {
        private readonly IUserAccessService _userAccessService;

        private readonly IUsersRepository _usersRepository;

        private readonly MockViewModels _mockViewModels;

        private readonly MockModels _mockModels;

        private readonly User _targetUser;

        private const string TARGER_USER = "TargetUser";

        private const string LOGGED_IN_USER = "LoggedInUser";

        public UserAccessServiceUnitTest()
        {
            var mockService = new MockServices();
            _userAccessService = mockService.GetService<IUserAccessService>();
            _usersRepository = mockService.GetService<IUsersRepository>();
            _mockModels = mockService.GetService<MockModels>();
            _mockViewModels = mockService.GetService<MockViewModels>();
            _targetUser = CreateTargetUser();
        }

        #region without userRoles
        [Fact]
        public async Task CanNotAccessAnyUserWithoutUserRoles()
        {
            var loggedInUser = _mockModels.LoggedInUser;
            loggedInUser.UserRoles = new List<UserRoles>();
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }
        #endregion

        #region without site userRoles
        [Fact]
        public async Task CanNotAccessAnyUserWithoutSiteUserRoles()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", "TestResource", 1);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserWithoutSiteUserRoles()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", "TestResource", 1);
            AddSiteToUser(TARGER_USER, 1, "TestSite");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessNonSiteUserHavingUserRoleWithoutSiteUserRoles()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", "TestResource", 1);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 1);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserHavingUserRoleWithoutSiteUserRoles()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", "TestResource", 1);
            AddSiteToUser(TARGER_USER, 1, "TestSite");
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 1);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }
        #endregion

        #region with site userRoles
        [Fact]
        public async Task CanNotAccessNonSiteUserWithSiteUserRoles()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 1);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserWithSiteUserRolesWithoutCommonSites()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 1);
            AddSiteToUser(TARGER_USER, 2, "TestSite2");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanAccessSiteUserWithSiteUserRolesWithCommonSites()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 5);
            AddSiteToUser(TARGER_USER, 1, "TestSite1");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task CanNotAccessNonSiteUserHavingNoCommonSiteUserRole()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "2", ResourceTypes.Site.ToString(), 5); ;
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessNonSiteUserHavingCommonSiteUserRoleWithLowerLevelAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 3);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessNonSiteUserHavingCommonSiteUserRoleWithHigherLevelAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 5);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserHavingNoCommonSiteAndNoCommonSiteUserRoles()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "2", ResourceTypes.Site.ToString(), 3);
            AddSiteToUser(TARGER_USER, 2, "TestSite2");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanAccessSiteUserHavingCommonSiteButNoCommonSiteUserRoles()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "2", ResourceTypes.Site.ToString(), 3);
            AddSiteToUser(TARGER_USER, 1, "TestSite1");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserHavingNoCommonSiteButCommonSiteUserRoleWithLowerLevelAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 3);
            AddSiteToUser(TARGER_USER, 2, "TestSite2");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserHavingNoCommonSiteButCommonSiteUserRoleWithHigherLevelAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 5);
            AddSiteToUser(TARGER_USER, 2, "TestSite2");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserHavingCommonSiteAndCommonSiteUserRoleWithLowerLevelAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 3);
            AddSiteToUser(TARGER_USER, 1, "TestSite1");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanAccessSiteUserHavingCommonSiteAndCommonSiteUserRoleWithHigherLevelAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 5);
            AddSiteToUser(TARGER_USER, 1, "TestSite1");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.True(result);
        }
        #endregion

        #region with all site userRoles
        [Fact]
        public async Task CanAccessNonSiteUserHavingNoSiteUserRoleWithAllSiteAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 4);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task CanAccessSiteUserHavingNoSiteUserRoleWithAllSiteAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 4);
            AddSiteToUser(TARGER_USER, 1, "TestSite1");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task CanNotAccessNonSiteUserHavingSiteUserRoleWithLowerAllSiteAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 3);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanAccessNonSiteUserHavingSiteUserRoleWithHigherAllSiteAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 5);
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task CanNotAccessSiteUserHavingSiteUserRoleWithLowerAllSiteAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 3);
            AddSiteToUser(TARGER_USER, 1, "TestSite1");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task CanAccessSiteUserHavingSiteUserRoleWithHigherAllSiteAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Site.ToString(), 4);
            AddUserRoleToUser(TARGER_USER, "1", ResourceTypes.Site.ToString(), 5);
            AddSiteToUser(TARGER_USER, 1, "TestSite1");
            var result = await _userAccessService.CanAccessUser(_targetUser.Id);
            Assert.True(result);
        }
        #endregion
        #region with hospice userRoles
        [Fact]
        public async Task CanAccessHospiceHavingHospiceUserRoleWithAllHospiceAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "*", ResourceTypes.Hospice.ToString(), 4);
            var result = await _userAccessService.CanAccessHospice(1);
            Assert.True(result);
        }

        [Fact]
        public async Task CanAccessHospiceHavingHospiceUserRoleWithSpecificHospiceAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "1", ResourceTypes.Hospice.ToString(), 4);
            var result = await _userAccessService.CanAccessHospice(1);
            Assert.True(result);
        }

        [Fact]
        public async Task CannotAccessHospiceIfNotHaveHospiceAccess()
        {
            AddUserRoleToUser(LOGGED_IN_USER, "2", ResourceTypes.Hospice.ToString(), 4);
            var result = await _userAccessService.CanAccessHospice(1);
            Assert.False(result);
        }

        [Fact]
        public async Task CannotAccessHospiceIfNotHaveAnyHospiceAccess()
        {
            var result = await _userAccessService.CanAccessHospice(1);
            Assert.False(result);
        }
        #endregion

        private void AddUserRoleToUser(string cognitoUserId, string resourceId, string resourceType, int roleLevel)
        {
            var user = _usersRepository.GetByCognitoUserId(cognitoUserId).Result;
            user.UserRoles = new List<UserRoles>()
            {
                new UserRoles()
                {
                    Id = new Random().Next(10000, 100000),
                    ResourceId = resourceId,
                    ResourceType = resourceType,
                    Role = new Roles()
                    {
                        Level = roleLevel
                    }
                }
            };
            _usersRepository.UpdateAsync(user);
        }

        private Users AddSiteToUser(string cognitoUserId, int id, string name)
        {
            var targetUser = _usersRepository.GetByCognitoUserId(cognitoUserId).Result;
            var siteMemberUser = new List<SiteMembers>();
            siteMemberUser.Add(new SiteMembers()
            {
                Id = id,
                SiteId = id,
                UserId = targetUser.Id,
                Site = new Sites()
                {
                    Id = id,
                    Name = name
                }
            });
            targetUser.SiteMembersUser = siteMemberUser;
            _usersRepository.UpdateAsync(targetUser).Wait();
            return targetUser;
        }

        private User CreateTargetUser()
        {
            var targetUserModel = new Users()
            {
                FirstName = "Target",
                LastName = "User",
                Email = "targetuser@example.com",
                CognitoUserId = "TargetUser"
            };
            _usersRepository.AddAsync(targetUserModel).Wait();

            var targetUser = new User()
            {
                Id = targetUserModel.Id,
                FirstName = targetUserModel.FirstName,
                LastName = targetUserModel.LastName,
                Email = targetUserModel.Email,
                UserId = targetUserModel.CognitoUserId
            };
            _mockViewModels.Users = _mockViewModels.Users.Append(targetUser);
            return targetUser;
        }
    }
}
