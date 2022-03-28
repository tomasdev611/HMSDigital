using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;

namespace HMSDigital.Patient.Test.MockProvider
{
    public class MockViewModels
    {
        public PatientCreateRequest GetPatientCreateRequest()
        {
            return new PatientCreateRequest()
            {
                FirstName = "create",
                LastName = "Patient",
                DateOfBirth = new DateTime(2010, 10, 10),
                HospiceId = 1,
                HospiceLocationId = 1,
                PatientHeight = 170,
                PatientWeight = 70,
                PatientAddress = GetPatientAddresses(),
                PhoneNumbers = GetPatientPhoneNumbers()
            };
        }

        public IEnumerable<PatientAddressRequest> GetPatientAddresses()
        {
            return new List<PatientAddressRequest>()
            {
                new PatientAddressRequest()
                {
                    AddressTypeId = 1,
                    Address = GetAddress()
                }
            };
        }

        public Address GetAddress()
        {
            return new Address()
            {
                AddressLine1 = "line 1",
                AddressLine2 = "line 2",
                City = "testCity",
                State = "testState",
                ZipCode = 12345
            };
        }

        public JsonPatchDocument<PatientDetail> GetPatientsJsonPatchDocument()
        {
            JsonPatchDocument<PatientDetail> patch = new JsonPatchDocument<PatientDetail>();
            patch.Replace(e => e.FirstName, "Patch");
            patch.Replace(e => e.LastName, "Patient");
            return patch;
        }

        public PatientStatusRequest GetPatientStatusRequest()
        {
            return new PatientStatusRequest()
            {
                PatientUuid = new Guid("B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351"),
                Status = "discharged",
                StatusChangedDate = DateTime.UtcNow,
                Reason = "Pending",
                HasOrders = false,   
                HasOpenOrders = true
            };
        }

        public PatientSearchRequest GetPatientSearchRequest()
        {
            return new PatientSearchRequest()
            {
                FirstName = "test",
                LastName = "patient",
                HospiceId = 1,

            };
        }

        public IEnumerable<PhoneNumberMinimal> GetPatientPhoneNumbers()
        {
            return new List<PhoneNumberMinimal>()
            {
                new PhoneNumberMinimal()
                {
                    Number = 1234567890,
                    NumberTypeId = 1
                }

            };
        }

        public IEnumerable<PatientStatusValidationRequest> GetPatientStatusValidationRequest()
        {
            return new List<PatientStatusValidationRequest>()
            {
                new PatientStatusValidationRequest()
                {
                     StatusId = 6,
                     PatientUuid = new Guid("B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351")
                }
            };
        }

        public IEnumerable<PatientStatusValidationRequest> GetPatientStatusValidationRequestForInvalidData()
        {
            return new List<PatientStatusValidationRequest>()
            {
                new PatientStatusValidationRequest()
                {
                     StatusId = 7,
                     PatientUuid = new Guid("B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351")
                }
            };
        }

        public PatientHospiceRequest GetPatientHospiceRequest()
        {
            return new PatientHospiceRequest()
            {
                HospiceId = 2,
                HospiceLocationId = 1
            };
        }

        public MergePatientRequest GetMergePatientRequest()
        {
            return new MergePatientRequest()
            {
                DuplicatePatientUUID = new Guid("B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351"),
                FirstName = "Merge",
                LastName = "Patient",
                DateOfBirth = new DateTime(2010, 10, 10),
                PatientHeight = 170,
                PatientWeight = 70
            };
        }

        public PatientSearchRequest GetPatientSearchRequestForInvalidData()
        {
            return new PatientSearchRequest()
            {
                FirstName = "Invalid",
                LastName = "patient",
                HospiceId = 10
            };
        }

        public PatientOrderRequest GetPatientOrderRequest()
        {
            return new PatientOrderRequest()
            {
                PatientUUID = new Guid("B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351"),
                HasDMEEquipment = false
            };
        }
    }
}
