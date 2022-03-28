using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class FacilityServiceUnitTest
    {
        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly IFacilityService _facilityService;

        private readonly IHospiceService _hospiceService;

        public FacilityServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _facilityService = mockService.GetService<IFacilityService>();
            _hospiceService = mockService.GetService<IHospiceService>();
            _sieveModel = new SieveModel();
        }

        [Fact]
        public async Task GetAllFacilitiesShouldReturnValidList()
        {
            var facilities = await _facilityService.GetAllFacilities(1, _sieveModel);
            Assert.NotEmpty(facilities.Records);
        }

        [Fact]
        public async Task ShouldGetFacilityForValidFacilityId()
        {
            var facility = await _facilityService.GetFacilityById(1);
            Assert.NotNull(facility);
        }

        [Fact]
        public async Task ShouldNotGetFacilityForInValidFacilityId()
        {
            var facility = await _facilityService.GetFacilityById(20);
            Assert.Null(facility);
        }

        [Fact]
        public async Task FacilityShouldBeCreatedForValidData()
        {
            var facilityCreateRequest = _mockViewModels.GetFacilityRequest();
            var createdFacility = await _facilityService.CreateFacility(facilityCreateRequest);
            var facility = await _facilityService.GetFacilityById(createdFacility.Id);

            Assert.Equal(facilityCreateRequest.Name, facility.Name);
            Assert.Equal(facilityCreateRequest.HospiceId, facility.HospiceId);
            Assert.Equal(facilityCreateRequest.Address.AddressLine1, facility.Address.AddressLine1);
            Assert.Equal(facilityCreateRequest.Address.AddressLine2, facility.Address.AddressLine2);
            Assert.Equal(facilityCreateRequest.Address.AddressLine3, facility.Address.AddressLine3);
            Assert.Equal(facilityCreateRequest.Address.City, facility.Address.City);
            Assert.Equal(facilityCreateRequest.Address.State, facility.Address.State);
            Assert.Equal(facilityCreateRequest.Address.ZipCode, facility.Address.ZipCode);
        }

        [Fact]
        public async Task FacilityCreationShouldFailForInvalidData()
        {
            var facilityCreateRequest = _mockViewModels.GetFacilityRequest();
            facilityCreateRequest.Name = "";
            await Assert.ThrowsAsync<ValidationException>(() => _facilityService.CreateFacility(facilityCreateRequest));
        }

        [Fact]
        public async Task FacilityCreationShouldFailForInvalidHospice()
        {
            var facilityCreateRequest = _mockViewModels.GetFacilityRequest();
            facilityCreateRequest.HospiceId = 10;
            var hospice = await _hospiceService.GetHospiceById(facilityCreateRequest.HospiceId);
            Assert.Null(hospice);
            await Assert.ThrowsAsync<ValidationException>(() => _facilityService.CreateFacility(facilityCreateRequest));
        }

        [Fact]
        public async Task FacilityShouldBeUpdatedForValidData()
        {
            var facilityUpdateRequest = _mockViewModels.GetFacilityJsonPatchDocument();
            var updatedHospiceLocation = await _facilityService.PatchFacility(1, facilityUpdateRequest);
            var facility = await _facilityService.GetFacilityById(updatedHospiceLocation.Id);
            var facilityPatch = facilityUpdateRequest.Operations.Where(o => o.path.Equals("/Name", StringComparison.OrdinalIgnoreCase))
                                        .FirstOrDefault();
            Assert.Equal(facilityPatch.value, facility.Name);
        }

        [Fact]
        public async Task FacilityUpdationShouldFailForInvalidData()
        {
            var facilityUpdateRequest = _mockViewModels.GetFacilityJsonPatchDocument();
            facilityUpdateRequest.Replace(h => h.HospiceId, 1);
            await Assert.ThrowsAsync<ValidationException>(() => _facilityService.PatchFacility(1, facilityUpdateRequest));
        }

        [Fact]
        public async Task GetAssignedPatientsShouldReturnValidList()
        {
            var facilityPatients = await _facilityService.GetAssignedPatients(1);
            Assert.NotEmpty(facilityPatients);
        }

        [Fact]
        public async Task SearchAssignedPatientsShouldReturnValidListOfPatientsAssignedToFacility()
        {
            var facilityId = 1;
            var assignedPatients = await _facilityService.SearchAssignedPatients(facilityId, _sieveModel);
            var patientOtherThanSpecifyFacility = assignedPatients.Where(p => p.FacilityId != facilityId);
            Assert.NotEmpty(assignedPatients);
            Assert.Empty(patientOtherThanSpecifyFacility);
        }

        [Fact]
        public async Task AssignPatientToFacilityShouldSuccessForValidData()
        {
            var facilityId = 1;
            var facilityPatientRequest = _mockViewModels.GetFacilityPatientRequest();
            var assignedPatient = await _facilityService.AssignPatientToFacilities(facilityPatientRequest);
            var facilityPatients = await _facilityService.GetAssignedPatients(facilityId);
            var assignedPatientsHistory = await _facilityService.SearchAssignedPatients(facilityId, _sieveModel);
            var facilityPatientAssociation = facilityPatients.Where(fp => fp.FacilityId == facilityId
                                                                && fp.PatientUuid == facilityPatientRequest.PatientUuid).FirstOrDefault();
            var facilityPatientAssociationHistory = assignedPatientsHistory.Where(fp => fp.FacilityId == facilityId
                                                                && fp.PatientUuid == facilityPatientRequest.PatientUuid).FirstOrDefault();
            Assert.NotNull(facilityPatientAssociation);
            Assert.NotNull(facilityPatientAssociationHistory);
            Assert.True(facilityPatientAssociationHistory.Active);
        }

        [Fact]
        public async Task AssignPatientToFacilityShouldFailForInvalidFacility()
        {
            var facilityPatientRequest = _mockViewModels.GetFacilityPatientRequest();
            facilityPatientRequest.FacilityPatientRooms = new List<FacilityPatientRoom>() { new FacilityPatientRoom() { FacilityId = 20, PatientRoomNumber = "A12345" } };
            var facility = await _facilityService.GetFacilityById(facilityPatientRequest.FacilityPatientRooms.Select(fpr => fpr.FacilityId).FirstOrDefault());
            Assert.Null(facility);
            await Assert.ThrowsAsync<ValidationException>(() => _facilityService.AssignPatientToFacilities(facilityPatientRequest));
        }
    }
}
