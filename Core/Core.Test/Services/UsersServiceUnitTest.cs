using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class UsersServiceUnitTest
    {
        private readonly IUserService _userService;

        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly MockModels _mockModels;

        public UsersServiceUnitTest()
        {
            var mockService = new MockServices();
            _userService = mockService.GetService<IUserService>();
            _sieveModel = new SieveModel();
            _mockViewModels = new MockViewModels();
            _mockModels = mockService.GetService<MockModels>();
        }

        #region User

        [Fact]
        public async Task GetAllUsersShouldReturnValidList()
        {
            var items = await _userService.GetAllUsers(_sieveModel);
            Assert.NotEmpty(items.Records);
        }

        [Fact]
        public async Task UserShouldBeEnable()
        {
            var user = await _userService.EnableUser(1);
            Assert.True(user.Enabled);
        }

        [Fact]
        public async Task UserShouldBeDisable()
        {
            var user = await _userService.DisableUser(1);
            Assert.False(user.Enabled);
        }

        [Fact]
        public async Task UserShouldBeCreatedForValidData()
        {
            var userRequest = _mockViewModels.GetUserCreateRequest();
            var createdUser = await _userService.CreateUser(userRequest);
            var user = await _userService.GetUserById(createdUser.Id);
            Assert.NotNull(user);
            Assert.Equal(userRequest.FirstName, user.FirstName);
            Assert.Equal(userRequest.LastName, user.LastName);
            Assert.Equal(userRequest.Email, user.Email);
            Assert.False(user.IsEmailVerified);
        }

        [Fact]
        public async Task UserShouldNotBeCreatedForAlreadyExistUser()
        {
            var userRequest = _mockViewModels.GetUserCreateRequest();
            userRequest.Email = "test.user1@test.com";
            var createdUser = await _userService.CreateUser(userRequest);
            var user = await _userService.GetUserById(createdUser.Id);
            Assert.NotNull(user);
            Assert.Equal(userRequest.Email, user.Email);
            Assert.False(user.IsEmailVerified);
        }

        [Fact]
        public async Task UserShouldBeFailedForInvalidData()
        {
            var userRequest = _mockViewModels.GetUserCreateRequest();
            userRequest.Email = "";
            await Assert.ThrowsAsync<ValidationException>(() => _userService.CreateUser(userRequest));
        }

        [Fact]
        public async Task UserShouldBeCreatedForValidDataUsingBulkUserCreate()
        {
            var bulkUserRequest = _mockViewModels.GetBulkUserCreateRequest();
            var createdUsers = await _userService.CreateBulkUser(bulkUserRequest);
            foreach (var createdUser in createdUsers)
            {
                var user = await _userService.GetUserById(createdUser.Id);
                var userRequest = bulkUserRequest.FirstOrDefault(r => r.Email == createdUser.Email);
                Assert.NotNull(user);
                Assert.Equal(userRequest.FirstName, user.FirstName);
                Assert.Equal(userRequest.LastName, user.LastName);
                Assert.Equal(userRequest.Email, user.Email);
                Assert.False(user.IsEmailVerified);
            }
        }

        [Fact]
        public async Task UserShouldBeUpdatedForValidData()
        {
            var userRequest = _mockViewModels.GetUsersUpdateRequest();
            var updatedUser = await _userService.UpdateUser(13, userRequest);
            var user = await _userService.GetUserById(updatedUser.Id);
            Assert.NotNull(user);
            Assert.Equal(userRequest.FirstName, user.FirstName);
            Assert.Equal(userRequest.LastName, user.LastName);
            Assert.Equal(userRequest.Email, user.Email);
            Assert.False(user.IsEmailVerified);
        }

        [Fact]
        public async Task UserShouldBeUpdatedOwnUserForValidData()
        {
            var userRequest = _mockViewModels.GetUsersUpdateRequest();
            var updatedUser = await _userService.UpdateMyUser(userRequest);
            var user = await _userService.GetMyUser();
            Assert.NotNull(user);
            Assert.Equal(userRequest.FirstName, user.FirstName);
            Assert.Equal(userRequest.LastName, user.LastName);
            Assert.Equal(userRequest.Email, user.Email);
            Assert.False(user.IsEmailVerified);
        }

        [Fact]
        public async Task UserUpdateShouldFailedForInvalidData()
        {
            var userRequest = _mockViewModels.GetUsersUpdateRequest();
            userRequest.Email = "";
            await Assert.ThrowsAsync<ValidationException>(() => _userService.UpdateUser(13, userRequest));
        }

        [Fact]
        public async Task UserUpdateShouldFailedForInvalidUser()
        {
            var userRequest = _mockViewModels.GetUsersUpdateRequest();
            await Assert.ThrowsAsync<ValidationException>(() => _userService.UpdateUser(134, userRequest));
        }

        [Fact]
        public async Task UserShouldNotBeEnableLowerRoleUserThanDisableUser()
        {
            var loggedInUser = _mockModels.LoggedInUser;
            loggedInUser.UserRoles = new List<UserRoles>()
            {
                new UserRoles()
                {
                    Id = new Random().Next(10000, 100000),
                    ResourceId = "*",
                    ResourceType = "Site",
                    Role = new Roles()
                    {
                        Level = 5
                    }
                }
            };
            await Assert.ThrowsAsync<ValidationException>(() => _userService.EnableUser(13));
        }

        [Fact]
        public async Task UserShouldNotBeEnableForInvalidUser()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _userService.EnableUser(130));
        }
        #endregion

        #region UserProfilePicture

        [Fact]
        public async Task UserProfilePictureShouldBeUploadedForValidData()
        {
            var userProfileRequest = _mockViewModels.GetUserPictureFileRequest();
            var updatedUser = await _userService.UpdateProfilePicture(13, userProfileRequest);
            var uploadedProfile = await _userService.ConfirmProfilePictureUpload(13);
            var userProfile = await _userService.GetProfilePicture(13);
            Assert.NotNull(userProfile);
        }

        [Fact]
        public async Task UserProfilePictureShouldBeUpdatedForValidData()
        {
            var oldUserProfile = await _userService.GetProfilePicture(12);
            Assert.NotNull(oldUserProfile);
            var userProfileRequest = _mockViewModels.GetUserPictureFileRequest();
            userProfileRequest.Name = "New Profile";
            var updatedUser = await _userService.UpdateProfilePicture(12, userProfileRequest);
            var uploadedProfile = await _userService.ConfirmProfilePictureUpload(12);
            var userProfile = await _userService.GetProfilePicture(12);
            Assert.NotNull(userProfile);
        }

        [Fact]
        public async Task UserProfilePictureShouldBeFailedToUploadForInvalidData()
        {
            var userProfileRequest = _mockViewModels.GetUserPictureFileRequest();
            userProfileRequest.ContentType = "";
            await Assert.ThrowsAsync<ValidationException>(() => _userService.UpdateProfilePicture(13, userProfileRequest));
        }

        [Fact]
        public async Task UserProfilePictureShouldBeRemovedForValidData()
        {
            var oldUserProfile = await _userService.GetProfilePicture(1234);
            Assert.NotNull(oldUserProfile);
            await _userService.RemoveProfilePicture(1234);
            var userProfile = await _userService.GetProfilePicture(1234);
            Assert.Null(userProfile);
        }
        #endregion
    }
}
