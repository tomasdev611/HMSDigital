using AutoMapper;
using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class HospiceMemberServiceUnitTest
    {
        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly MockModels _mockModels;

        private readonly HospiceMemberService _hospiceMemberService;

        private readonly IUserService _userService;

        private readonly IRolesRepository _rolesRepository;

        private readonly IMapper _mapper;
        public HospiceMemberServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _hospiceMemberService = mockService.GetService<HospiceMemberService>();
            _userService = mockService.GetService<IUserService>();
            _rolesRepository = mockService.GetService<MockRepository>().GetRolesRepository();
            _sieveModel = new SieveModel();
            _mockModels = mockService.GetService<MockModels>();
            _mapper = mockService.GetService<IMapper>();
        }

        [Fact]
        public async Task GetAllHospiceMembersShouldReturnValidList()
        {
            var hospiceMembers = await _hospiceMemberService.GetAllHospiceMembers(1, _sieveModel, "");
            Assert.NotEmpty(hospiceMembers.Records);
        }
        [Fact]
        public async Task ShouldGetHospiceMemberForValidMemberId()
        {
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, 1);
            Assert.NotNull(hospiceMember);
        }

        [Fact]
        public async Task ShouldNotGetHospiceMemberForInValidMemberId()
        {
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, 10);
            Assert.Null(hospiceMember);
        }

        [Fact]
        public async Task HospiceMemberShouldBeCreateForValidData()
        {
            var hospiceMemberRequest = _mockViewModels.GetHospiceMemberRequest();
            var createdHospiceMember = await _hospiceMemberService.CreateHospiceMember(1, hospiceMemberRequest);
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, createdHospiceMember.Id);

            Assert.Equal(hospiceMemberRequest.FirstName, hospiceMember.FirstName);
            Assert.Equal(hospiceMemberRequest.LastName, hospiceMember.LastName);
            Assert.Equal(hospiceMemberRequest.Email, hospiceMember.Email);
            Assert.Equal(hospiceMemberRequest.PhoneNumber, hospiceMember.PhoneNumber);
        }

        [Fact]
        public async Task HospiceMemberCreationShouldFailedForInvalidData()
        {
            var hospiceMemberRequest = _mockViewModels.GetHospiceMemberRequest();
            hospiceMemberRequest.Email = "";
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceMemberService.CreateHospiceMember(1, hospiceMemberRequest));
        }

        [Fact]
        public async Task HospiceMemberShouldBeCreateForValidHospiceRole()
        {
            var hospiceMemberRequest = _mockViewModels.GetHospiceMemberRequest();
            hospiceMemberRequest.UserRoles.FirstOrDefault().RoleId = 2;
            var createdHospiceMember = await _hospiceMemberService.CreateHospiceMember(1, hospiceMemberRequest);
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, createdHospiceMember.Id);
            var role = await _rolesRepository.GetByIdAsync(hospiceMemberRequest.UserRoles.FirstOrDefault().RoleId ?? 0);

            Assert.NotNull(role);
            Assert.Equal("Hospice", role.RoleType);
            Assert.Equal(hospiceMemberRequest.FirstName, hospiceMember.FirstName);
            Assert.Equal(hospiceMemberRequest.LastName, hospiceMember.LastName);
            Assert.Equal(hospiceMemberRequest.Email, hospiceMember.Email);
            Assert.Equal(hospiceMemberRequest.PhoneNumber, hospiceMember.PhoneNumber);
        }

        [Fact]
        public async Task HospiceMemberCreationForInternalUserShouldSucceed()
        {
            var loggedInUser = _mapper.Map<User>(_mockModels.LoggedInUser);
            var createdHospiceMember = await _hospiceMemberService.CreateInternalHospiceMember(loggedInUser);
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(createdHospiceMember.HospiceId, createdHospiceMember.Id);

            Assert.Equal(loggedInUser.FirstName, hospiceMember.FirstName);
            Assert.Equal(loggedInUser.LastName, hospiceMember.LastName);
            Assert.Equal(loggedInUser.Email, hospiceMember.Email);
            Assert.Equal(loggedInUser.PhoneNumber, hospiceMember.PhoneNumber);
        }

        [Fact]
        public async Task HospiceMemberDeletionForInternalUserShouldSucceed()
        {
            var loggedInUser = _mapper.Map<User>(_mockModels.LoggedInUser);
            var createdHospiceMember = await _hospiceMemberService.CreateInternalHospiceMember(loggedInUser);
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(createdHospiceMember.HospiceId, createdHospiceMember.Id);
            Assert.NotNull(hospiceMember);

            await _hospiceMemberService.DeleteInternalHospiceMember(loggedInUser.Id);
            var deletedHospiceMember = await _hospiceMemberService.GetHospiceMemberById(createdHospiceMember.HospiceId, createdHospiceMember.Id);

            Assert.Null(deletedHospiceMember);
        }

        [Fact]
        public async Task HospiceMemberCreationShouldFailedForInvalidRoleId()
        {
            var hospiceMemberRequest = _mockViewModels.GetHospiceMemberRequest();
            hospiceMemberRequest.LastName = "member user test";
            hospiceMemberRequest.UserRoles.FirstOrDefault().RoleId = 20;
            var role = await _rolesRepository.GetByIdAsync(hospiceMemberRequest.UserRoles.FirstOrDefault().RoleId ?? 0);

            Assert.Null(role);
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceMemberService.CreateHospiceMember(1, hospiceMemberRequest));
            var usersResponse = await _userService.SearchUsers(hospiceMemberRequest.LastName, _sieveModel);
            Assert.Empty(usersResponse.Records);

        }

        [Fact]
        public async Task HospiceMemberCreationShouldFailedForNonHospiceRole()
        {
            var hospiceMemberRequest = _mockViewModels.GetHospiceMemberRequest();
            hospiceMemberRequest.UserRoles.FirstOrDefault().RoleId = 1;
            var role = await _rolesRepository.GetByIdAsync(hospiceMemberRequest.UserRoles.FirstOrDefault().RoleId ?? 0);

            Assert.NotEqual("Hospice", role.RoleType);
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceMemberService.CreateHospiceMember(1, hospiceMemberRequest));
        }

        [Fact]
        public async Task HospiceMemberShouldBeUpdatedForValidData()
        {
            var hospiceMemberUpdateRequest = _mockViewModels.GetUpdateHospiceMemberRequest();
            var updatedhospiceMember = await _hospiceMemberService.UpdateHospiceMember(1, 1, hospiceMemberUpdateRequest);
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, updatedhospiceMember.Id);

            Assert.Equal(hospiceMemberUpdateRequest.FirstName, hospiceMember.FirstName);
            Assert.Equal(hospiceMemberUpdateRequest.LastName, hospiceMember.LastName);
            Assert.Equal(hospiceMemberUpdateRequest.Email, hospiceMember.Email);
            Assert.Equal(hospiceMemberUpdateRequest.PhoneNumber, hospiceMember.PhoneNumber);
        }

        [Fact]
        public async Task HospiceMemberUpdationShouldFailedForInvalidData()
        {
            var hospiceMemberUpdateRequest = _mockViewModels.GetUpdateHospiceMemberRequest();
            hospiceMemberUpdateRequest.Email = "";
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceMemberService.UpdateHospiceMember(1, 1, hospiceMemberUpdateRequest));
        }

        [Fact]
        public async Task HospiceMemberUpdationShouldFailedForInvalidRole()
        {
            var hospiceMemberUpdateRequest = _mockViewModels.GetUpdateHospiceMemberRequest();
            hospiceMemberUpdateRequest.LastName = "member user test";
            hospiceMemberUpdateRequest.UserRoles = new List<HospiceMemberRoleRequest>()
            {
                new HospiceMemberRoleRequest()
                {
                    RoleId = 20
                }
            };
            var role = await _rolesRepository.GetByIdAsync(hospiceMemberUpdateRequest.UserRoles.FirstOrDefault().RoleId ?? 0);

            Assert.Null(role);
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceMemberService.UpdateHospiceMember(1, 1, hospiceMemberUpdateRequest));
            var usersResponse = await _userService.SearchUsers(hospiceMemberUpdateRequest.LastName, _sieveModel);
            Assert.Empty(usersResponse.Records);
        }

        [Fact]
        public async Task HospiceMemberCreationForCsvRequestShouldSucceed()
        {
            var hospiceMemberRequests = new List<HospiceMemberCsvRequest>()
            {
                new HospiceMemberCsvRequest()
                {
                    FirstName= "Test",
                    LastName = "Member",
                    Role = "HospiceAdmin",
                    CountryCode = 1,
                    PhoneNumber = 9876543210,
                    Email = "abc@example.com"
                }
            };

            var (createdHospiceMembers, validationResults) = await _hospiceMemberService.CreateHospiceMembers(1, hospiceMemberRequests, false);
            Assert.Null(validationResults);
            Assert.NotNull(createdHospiceMembers);

            var createdHospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, createdHospiceMembers.FirstOrDefault().Id);
            var hospiceMemberRequest = hospiceMemberRequests.FirstOrDefault();
            Assert.Equal(hospiceMemberRequest.FirstName, createdHospiceMember.FirstName);
            Assert.Equal(hospiceMemberRequest.LastName, createdHospiceMember.LastName);
            Assert.Equal(hospiceMemberRequest.Email, createdHospiceMember.Email);
            Assert.Equal(hospiceMemberRequest.PhoneNumber, createdHospiceMember.PhoneNumber);
        }

        [Fact]
        public async Task UpsertApproverForHospiceShouldSucceed()
        {
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, 1);
            Assert.False(hospiceMember.CanApproveOrder);
            var approverRequest = new HMSDigital.Core.ViewModels.NetSuite.ApproverRequest()
            {
                NetSuiteCustomerId = 1,
                Approvers = new List<int>() { hospiceMember.NetSuiteContactId }
            };
            await _hospiceMemberService.UpsertApprovers(approverRequest);

            var updatedHospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, hospiceMember.Id);
            Assert.True(updatedHospiceMember.CanApproveOrder);
        }

        [Fact]
        public async Task UpsertApproverForHospiceLocationShouldSucceed()
        {
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, 2);
            Assert.False(hospiceMember.CanApproveOrder);
            var approverRequest = new HMSDigital.Core.ViewModels.NetSuite.ApproverRequest()
            {
                NetSuiteCustomerId = 2020,
                Approvers = new List<int>() { 2 }
            };
            await _hospiceMemberService.UpsertApprovers(approverRequest);

            var updatedHospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, hospiceMember.Id);
            Assert.True(updatedHospiceMember.HospiceLocationMembers.FirstOrDefault().CanApproveOrder);
        }

        [Fact]
        public async Task UpsertApproverShouldFailForInvalidCustomerId()
        {
            var approverRequest = new HMSDigital.Core.ViewModels.NetSuite.ApproverRequest()
            {
                NetSuiteCustomerId = 123456
            };
            await Assert.ThrowsAsync<ValidationException>(() => _hospiceMemberService.UpsertApprovers(approverRequest));
        }

        [Fact]
        public async Task DeleteHospiceMemberShouldSucceed()
        {
            var hospiceMember = await _hospiceMemberService.GetHospiceMemberById(1, 2);
            await _hospiceMemberService.DeleteHospiceMember(hospiceMember.HospiceId, hospiceMember.Id);
            var deletedHospiceMember = await _hospiceMemberService.GetHospiceMemberById(hospiceMember.HospiceId, hospiceMember.Id);
            Assert.Null(deletedHospiceMember);
        }

        [Fact]
        public async Task UpdateHospiceMemberInNetSuiteShouldSucceed()
        {
            Assert.Empty(_mockModels.CustomerContactRequests);
            await _hospiceMemberService.UpdateHospiceMemberInNetSuite(1);
            Assert.NotEmpty(_mockModels.CustomerContactRequests);
        }
    }
}
