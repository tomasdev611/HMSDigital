using HMSDigital.Patient.BusinessLayer.Interfaces;
using HMSDigital.Patient.Test.MockProvider;
using Sieve.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HMSDigital.Patient.Test.Services
{
    public class PatientV2ServiceUnitTest
    {
        private readonly IPatientV2Service _patientV2Service;

        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly MockModels _mockModels;

        public PatientV2ServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _patientV2Service = mockService.GetService<IPatientV2Service>();
            _sieveModel = new SieveModel();
            _mockModels = new MockModels();
        }

        [Fact]
        public async Task GetAllPatientsShouldReturnValidList()
        {
            var patientsResult = await _patientV2Service.GetAllPatients(_sieveModel);
            Assert.NotEmpty(patientsResult.Records);
        }

        [Fact]
        public async Task GetPatientByIdShouldReturnValidPatient()
        {
            var expectedPatientDetails = _mockModels.GetPatient(1);
            var patientDetils = await _patientV2Service.GetPatientById(1);
            Assert.Equal(expectedPatientDetails.FirstName, patientDetils.FirstName);
            Assert.Equal(expectedPatientDetails.LastName, patientDetils.LastName);
            Assert.Equal(expectedPatientDetails.DateOfBirth, patientDetils.DateOfBirth);
            Assert.Equal(expectedPatientDetails.HospiceId, patientDetils.HospiceId);
            Assert.Equal(expectedPatientDetails.HospiceLocationId, patientDetils.HospiceLocationId);
            Assert.Equal(expectedPatientDetails.PatientHeight.ToString(), patientDetils.PatientHeight.ToString());
            Assert.Equal(expectedPatientDetails.PatientWeight, patientDetils.PatientWeight);
        }

        [Fact]
        public async Task SearchPatientsShouldReturnValidList()
        {
            var patientSearchRequest = _mockViewModels.GetPatientSearchRequest();
            var patientsResult = await _patientV2Service.SearchPatients(patientSearchRequest);
            Assert.NotEmpty(patientsResult);
        }

        [Fact]
        public async Task SearchPatientsBySearchQueryShouldReturnValidList()
        {
            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, "test");
            Assert.NotEmpty(patientsResult.Records);
        }

        [Fact]
        public async Task SearchPatientsBySearchQueryShouldNotBeCaseSensitive()
        {
            var patientsResultforTeSt = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, "TeSt");
            var patientsResultfortest = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, "test");
            Assert.Equal(patientsResultforTeSt.Records.Count() , patientsResultfortest.Records.Count());
        }

        [Fact]
        public async Task SearchPatientsByEmptySearchQueryShouldReturnNull()
        {
            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, "");
            Assert.Null(patientsResult);
        }


        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("       ")]
        [InlineData("\n")]
        public async Task SearchPatientsSearchQueryShouldReturnNull(string searchQuery)
        {
            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.Null(patientsResult);
        }       

        [Fact]
        public async Task SearchPatientsByUniqueIdShouldReturnValidList()
        {
            string searchQuery = "B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351";

            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal(searchQuery.ToUpper(), patientsResult.Records.First().UniqueId.ToUpper());
        }

        [Fact]
        public async Task SearchPatientsByFirstNameLastNameShouldReturnValidList()
        {
            string searchQuery = "test Patient";

            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal("test", patientsResult.Records.First().FirstName);
            Assert.Equal("Patient", patientsResult.Records.First().LastName);
        }        

        [Fact]
        public async Task SearchPatientsByFirstNameLastNameShouldNotBeCaseSensitive()
        {
            string searchQuery = "TeSt pAtIeNt";

            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal("test", patientsResult.Records.First().FirstName);
            Assert.Equal("Patient", patientsResult.Records.First().LastName);
        }

        [Fact]
        public async Task SearchPatientsByLastNameFirstNameShouldReturnValidList()
        {
            string searchQuery = "Patient test";

            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal("test", patientsResult.Records.First().FirstName);
            Assert.Equal("Patient", patientsResult.Records.First().LastName);
        }

        [Fact]
        public async Task SearchPatientsByLastNameFirstNameShouldNotBeCaseSensitive()
        {
            string searchQuery = "pAtIeNt TeSt";

            var patientsResult = await _patientV2Service.SearchPatientsBySearchQuery(_sieveModel, searchQuery);
            Assert.NotEmpty(patientsResult.Records);
            Assert.Equal("test", patientsResult.Records.First().FirstName);
            Assert.Equal("Patient", patientsResult.Records.First().LastName);
        }                 

        [Fact]
        public async Task SearchPatientsShouldReturnEmptyList()
        {
            var patientSearchRequest = _mockViewModels.GetPatientSearchRequestForInvalidData();
            var patientsResult = await _patientV2Service.SearchPatients(patientSearchRequest);
            Assert.Empty(patientsResult);
        }

        [Fact]
        public async Task CanPlaceOrder()
        {
            var patientOrderRequest = _mockViewModels.GetPatientOrderRequest();
            await _patientV2Service.RecordPatientOrder(patientOrderRequest);
            Assert.True(true);
        }
    }
}
