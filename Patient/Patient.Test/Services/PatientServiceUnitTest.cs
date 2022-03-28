using HMSDigital.Patient.BusinessLayer.Interfaces;
using HMSDigital.Patient.Data.Enums;
using HMSDigital.Patient.Test.MockProvider;
using Sieve.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HMSDigital.Patient.Test.Services
{
    public class PatientServiceUnitTest
    {
        private readonly IPatientService _patientService;

        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        public PatientServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _patientService = mockService.GetService<IPatientService>();
            _sieveModel = new SieveModel();
        }

        [Fact]
        public async Task GetAllPatientsShouldReturnValidList()
        {
            var patientsResult = await _patientService.GetAllPatients(_sieveModel);
            Assert.NotEmpty(patientsResult.Records);
        }

        [Fact]
        public async Task PatientShouldBeCreateForValidPatient()
        {
            var patientCreateRequest = _mockViewModels.GetPatientCreateRequest();
            var createdPatient = await _patientService.CreatePatient(patientCreateRequest);
            var patient = await _patientService.GetPatientById(createdPatient.Id);

            var requestedAddress = patientCreateRequest.PatientAddress.Select(pa => pa.Address).FirstOrDefault();
            var actualAddress = patient.PatientAddress.Select(pa => pa.Address).FirstOrDefault();

            Assert.Equal(patientCreateRequest.FirstName, patient.FirstName);
            Assert.Equal(patientCreateRequest.LastName, patient.LastName);
            Assert.Equal(patientCreateRequest.DateOfBirth, patient.DateOfBirth);
            Assert.Equal(patientCreateRequest.HospiceId, patient.HospiceId);
            Assert.Equal(requestedAddress.AddressLine1, actualAddress.AddressLine1);
            Assert.Equal(requestedAddress.AddressLine2, actualAddress.AddressLine2);
            Assert.Equal(requestedAddress.City, actualAddress.City);
            Assert.Equal(requestedAddress.ZipCode, actualAddress.ZipCode);
        }

        [Fact]
        public async Task PatientShouldBeUpdatedForValidData()
        {
            var patientPatchRequest = _mockViewModels.GetPatientsJsonPatchDocument();
            var updatedPatient = await _patientService.PatchPatient(1, patientPatchRequest, false);
            var patient = await _patientService.GetPatientById(updatedPatient.Id);

            Assert.Equal("Patch", patient.FirstName);
            Assert.Equal("Patient", patient.LastName);
        }

        [Fact]
        public async Task SearchPatientsShouldReturnValidList()
        {
            var patientSearchRequest = _mockViewModels.GetPatientSearchRequest();
            var patientsResult = await _patientService.SearchPatients(patientSearchRequest);
            Assert.NotEmpty(patientsResult);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("TeSt")]
        [InlineData("TEST")]
        public async Task SearchPatientsBySearchQueryShouldReturnValidList(string searchQuery)
        {
            var patientsResult = await _patientService.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.NotEmpty(patientsResult.Records);
        }
      
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("       ")]
        [InlineData("\n")]
        public async Task SearchPatientsSearchQueryShouldReturnNull(string searchQuery)
        {
            var patientsResult = await _patientService.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.Null(patientsResult);
        }

        [Fact]
        public async Task SearchPatientsByBirthDateShouldReturnValidList()
        {
            string searchQueryDate = "01/01/2000";
            DateTime patientBirthDate = DateTime.Parse(searchQueryDate);

            var patientsResult = await _patientService.SearchPatientsBySearchQuery(_sieveModel, searchQueryDate);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal(patientBirthDate, patientsResult.Records.First().DateOfBirth);
        }

        [Fact]
        public async Task SearchPatientsByBirthYearShouldReturnValidList()
        {
            string searchQueryYear = "2000";
            int patientBirthYear = int.Parse(searchQueryYear);

            var patientsResult = await _patientService.SearchPatientsBySearchQuery(_sieveModel, searchQueryYear);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal(patientBirthYear, patientsResult.Records.First().DateOfBirth.Year);
        }

        [Fact]
        public async Task SearchPatientsByUniqueIdShouldReturnValidList()
        {
            string searchQueryUniqueId = "B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351";

            var patientsResult = await _patientService.SearchPatientsBySearchQuery(_sieveModel, searchQueryUniqueId);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal(searchQueryUniqueId.ToUpper(), patientsResult.Records.First().UniqueId.ToUpper());
        }

        [Theory]
        [InlineData("test", "Patient")]
        [InlineData("TeSt", "pAtIeNt")]
        [InlineData("Patient", "test")]
        [InlineData("pAtIeNt", "TeSt")]
        public async Task SearchPatientsByFirstNameLastNameShouldReturnValidList(string firstName , string lastName)
        {
            string searchQuery = firstName + " " + lastName;

            var patientsResult = await _patientService.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal("test", patientsResult.Records.First().FirstName);
            Assert.Equal("Patient", patientsResult.Records.First().LastName);
        }
      

        [Fact]
        public async Task PatientStatusShouldBeValidated()
        {
            var patientValidationStatusRequest = _mockViewModels.GetPatientStatusValidationRequest();
            var patientStatuses = await _patientService.ValidatePatientStatus(patientValidationStatusRequest, false);
            Assert.True(patientStatuses.First().HasValidStatus);
        }       

        [Fact]
        public async Task PatientStatusShouldBeUpdated()
        {
            var patientStatusRequest = _mockViewModels.GetPatientStatusRequest();
            await _patientService.UpdatePatientStatus(1, patientStatusRequest);
            var patientDetail = await _patientService.GetPatientById(1);
            Assert.Equal(patientStatusRequest.Status.ToUpper(), patientDetail.Status.ToUpper());
        }

        [Fact]
        public async Task PatientStatusShouldBeUpdatedByPatientUuid()
        {
            var patientDetails = await _patientService.GetPatientById(1);
            var patientStatusRequest = _mockViewModels.GetPatientStatusRequest();
            await _patientService.UpdatePatientStatusByPatientUuid(new Guid(patientDetails.UniqueId), patientStatusRequest);
            var patientDetail = await _patientService.GetPatientById(1);
            Assert.Equal(patientStatusRequest.Status.ToUpper(), patientDetail.Status.ToUpper());
        }

        [Fact]
        public async Task PatientHospiceIdShouldBeUpdated()
        {
            var patientHospiceRequest = _mockViewModels.GetPatientHospiceRequest();
            var patientDetails = await _patientService.GetPatientById(1);
            await _patientService.UpdatePatientHospiceByPatientUuid(new Guid(patientDetails.UniqueId), patientHospiceRequest);
            var updatedPatientDetails = await _patientService.GetPatientById(1);
            Assert.Equal(patientHospiceRequest.HospiceId, updatedPatientDetails.HospiceId);
            Assert.Equal(patientHospiceRequest.HospiceLocationId, updatedPatientDetails.HospiceLocationId);
        }

        [Fact]
        public async Task ShouldGetCorrectPatientAddress()
        {
            var address = _mockViewModels.GetAddress();
            var patientAddress = await _patientService.GetPatientAddressByUuid(new Guid("2BD67540-3D24-4904-A35D-D2B9F7BE3883"));
            Assert.Equal(address.AddressLine1, patientAddress.AddressLine1);
            Assert.Equal(address.AddressLine2, patientAddress.AddressLine2);
            Assert.Equal(address.ZipCode, patientAddress.ZipCode);
            Assert.Equal(address.State, patientAddress.State);
            Assert.Equal(address.City, patientAddress.City);
        }
       
        [Fact]
        public async Task PatientV2ShouldBeCreateForValidPatient()
        {
            var patientCreateRequest = _mockViewModels.GetPatientCreateRequest();
            var createdPatient = await _patientService.CreatePatientV2(patientCreateRequest);
            var patient = await _patientService.GetPatientById(createdPatient.Id);

            var requestedAddress = patientCreateRequest.PatientAddress.Select(pa => pa.Address).FirstOrDefault();
            var actualAddress = patient.PatientAddress.Select(pa => pa.Address).FirstOrDefault();

            Assert.Equal(patientCreateRequest.FirstName, patient.FirstName);
            Assert.Equal(patientCreateRequest.LastName, patient.LastName);
            Assert.Equal(patientCreateRequest.DateOfBirth, patient.DateOfBirth);
            Assert.Equal(patientCreateRequest.HospiceId, patient.HospiceId);
            Assert.Equal(requestedAddress.AddressLine1, actualAddress.AddressLine1);
            Assert.Equal(requestedAddress.AddressLine2, actualAddress.AddressLine2);
            Assert.Equal(requestedAddress.City, actualAddress.City);
            Assert.Equal(requestedAddress.ZipCode, actualAddress.ZipCode);
        }

        [Fact]
        public async Task PatietShouldMoveToHospice()
        { 
            var patientDetail = await _patientService.GetPatientById(1);
            var createdPatient = await _patientService.MovePatientToHospiceLocation(new Guid(patientDetail.UniqueId) ,2 , 1, DateTime.UtcNow.AddDays(1));
            var updatedPatientDetail = await _patientService.GetPatientById(createdPatient.Id);
            Assert.Equal(2, updatedPatientDetail.HospiceId);
            Assert.Equal(1, updatedPatientDetail.HospiceLocationId);
        }

        [Fact]
        public async Task PatientDetailsShouldGetByUuid()
        {
            var patientDetails = await _patientService.GetPatientById(1);
            var patientResult = await _patientService.GetPatientByPatientUuid(patientDetails.UniqueId);
            Assert.Equal(patientDetails.FirstName, patientResult.FirstName);
            Assert.Equal(patientDetails.LastName, patientResult.LastName);            
        }

        [Fact]
        public async Task PatientDetailsShouldNullByInvalidUuid()
        {
            var patientResult = await _patientService.GetPatientByPatientUuid("0000");
            Assert.Null(patientResult);
        }

        [Fact]
        public async Task PaitientShouldNullForInvalidId()
        {
            var patient = await _patientService.GetPatientById(2);
            Assert.Null(patient);
        }

        [Fact]
        public async Task ShouldGetNullPatientAddressForInvalidUuid()
        {
            var patientAddress = await _patientService.GetPatientAddressByUuid(new Guid("2BD67549-3D24-4904-A35D-D2B9F7BE3883"));
            Assert.Null(patientAddress);
        }

        [Fact]
        public async Task PatientStatusShouldBeValidatedForInvalidStatus()
        {
            var patientValidationStatusRequest = _mockViewModels.GetPatientStatusValidationRequestForInvalidData();
            var patientStatuses = await _patientService.ValidatePatientStatus(patientValidationStatusRequest, false);
            Assert.False(patientStatuses.First().HasValidStatus);
        }

        [Fact]
        public async Task ShouldGetPatientNotes()
        {
            var patientNotes = await _patientService.GetPatientNotes(1, true);
            var note = patientNotes.First();
            Assert.Equal("Test Note", note.Note);
            Assert.Equal(1, note.CreatedByUserId);
            Assert.Equal(new DateTime(2020,1,1).ToString(), note.CreatedDateTime.ToString());            
        }

        [Fact]
        public async Task PatientStatusShouldGetFixed()
        {
            var patientStatusRequest = _mockViewModels.GetPatientStatusRequest();
            await _patientService.FixPatientStatus(patientStatusRequest);
            var patientDetails = await _patientService.GetPatientById(1);
            Assert.Equal((int)PatientStatusTypes.Pending, patientDetails.StatusId);           
        }

        [Fact]
        public async Task GetAllPatientLookupShouldReturnValidList()
        {
            var patientsResult = await _patientService.GetAllPatientLookUp(_sieveModel);
            Assert.NotEmpty(patientsResult.Records);
        }

        [Fact]
        public async Task NonVerifiedAddressShouldGetFixed()
        {
            var address = await _patientService.FixNonVerifiedAddress(1);
            Assert.True(address.IsVerified);
        }       

        [Fact]
        public async Task MissingFhirPatientShouldGetFixed()
        {
            var count = await _patientService.FixMissingFhirPatients();
            Assert.NotEqual(0, count);
        }

        [Fact]
        public async Task PatientHms2IdShouldGetUpdated()
        {
            var patientDetails = await _patientService.GetPatientById(1);
            await _patientService.UpdateHms2PatientId(new Guid(patientDetails.UniqueId), 100);
            var updatedPatientDetails = await _patientService.GetPatientById(1);
            Assert.Equal(100, updatedPatientDetails.Hms2Id);
        }

        [Fact]
        public async Task GetNonVerifiedAddresses()
        {
            var addresses = await _patientService.GetNonVerifiedAddresses(_sieveModel);
            Assert.Equal(0, addresses.TotalRecordCount);
        }
        
        [Fact]
        public async Task PatientShouldGetMerged()
        {
            var patientMergeRequest = _mockViewModels.GetMergePatientRequest();
            var patientDetails = await _patientService.GetPatientById(1);
            await _patientService.MergePatients(new Guid(patientDetails.UniqueId), patientMergeRequest);
            var updatedPatientDetails = await _patientService.GetPatientById(1);
            Assert.Equal(patientMergeRequest.FirstName, updatedPatientDetails.FirstName);
            Assert.Equal(patientMergeRequest.LastName, updatedPatientDetails.LastName);
            Assert.Equal(patientMergeRequest.DateOfBirth, updatedPatientDetails.DateOfBirth);
            Assert.Equal(patientMergeRequest.PatientHeight, updatedPatientDetails.PatientHeight);
            Assert.Equal(patientMergeRequest.PatientWeight, updatedPatientDetails.PatientWeight);
        }
    }
}
