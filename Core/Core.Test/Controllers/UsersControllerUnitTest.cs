using Core.Test.MockProvider;
using HMSDigital.Common.API.Auth;
using HMSDigital.Core.API.Controllers;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Controllers
{
    public class UsersControllerUnitTest
    {
        private readonly UsersController _usersController;

        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly MockServices _mockController;

        public UsersControllerUnitTest()
        {
            _mockController = new MockServices();
            _usersController = _mockController.GetController<UsersController>();
            _sieveModel = new SieveModel();
            _mockViewModels = new MockViewModels();
        }


        [Fact]
        public async Task CanAccessUserListIfHavingCanReadUserPermission()
        {
            _mockController.TestAuthorizeAttribute<UsersController>(_usersController, "GetUsers", PolicyConstants.CAN_READ_USER, new Type[] { typeof(SieveModel) });
        }

        [Fact]
        public async Task CanCreateUserIfHavingCanCreateUserPermission()
        {
            _mockController.TestAuthorizeAttribute<UsersController>(_usersController, "CreateUser", PolicyConstants.CAN_CREATE_USER, new Type[] { typeof(UserCreateRequest) });
        }

        [Fact]
        public async Task CanEnableUserIfHavingCanManageUserAccessPermission()
        {
            _mockController.TestAuthorizeAttribute<UsersController>(_usersController, "EnableUser", PolicyConstants.CAN_MANAGE_USER_ACCESS, new Type[] { typeof(int)});
        }

        [Fact]
        public async Task CanDisableUserIfHavingCanManageUserAccessPermission()
        {
            _mockController.TestAuthorizeAttribute<UsersController>(_usersController, "DisableUser", PolicyConstants.CAN_MANAGE_USER_ACCESS, new Type[] { typeof(int) });
        }

        [Fact]
        public async Task CanUpdateUserIfHavingCanUpdateUserPermission()
        {
            _mockController.TestAuthorizeAttribute<UsersController>(_usersController, "UpdateUser", PolicyConstants.CAN_UPDATE_USER, new Type[] { typeof(int), typeof(UserMinimal) });
        }
       
    }
}
