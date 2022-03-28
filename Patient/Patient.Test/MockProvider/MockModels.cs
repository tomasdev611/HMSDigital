using System;
using System.Collections.Generic;
using System.Linq;
using HMSDigital.Patient.Data.Models;

namespace HMSDigital.Patient.Test.MockProvider
{

    public class MockModels
    {
        public IEnumerable<PatientDetails> Patients { get; set; }

        public IEnumerable<Addresses> Addresses { get; set; }

        public MockModels()
        {
            Patients = GetPatients();
            Addresses = GetAddresses();
        }

        public IEnumerable<PatientDetails> GetPatients()
        {
            return new List<PatientDetails>()
            {
                GetPatient(1)
            };
        }

        public PatientDetails GetPatient(int id)
        {
            return new PatientDetails()
            {
                Id = id,
                FirstName = "test",
                LastName = "Patient",
                DateOfBirth = new DateTime(2000, 01, 01),
                HospiceId = 1,
                HospiceLocationId = 1,
                PatientHeight = 150,
                PatientWeight = 60,
                PhoneNumbers = GetPhoneNumbers().ToList(),
                CreatedByUserId = 1,
                CreatedDateTime = DateTime.UtcNow,
                UniqueId = new Guid("B3AA0DC1-AC9C-4DA1-97A1-FA852B37F351"),
                PatientNotes = GetPatientNotes()
            };
        }

        public IEnumerable<PhoneNumbers> GetPhoneNumbers()
        {
            return new List<PhoneNumbers>()
            {
                new PhoneNumbers()
                {
                    Number = 1234567890,
                    NumberTypeId = 1
                }
            };
        }

        public List<Addresses> GetAddresses()
        {
            return new List<Addresses>()
            {
                new Addresses()
                {
                    AddressLine1 = "line 1",
                    AddressLine2 = "line 2",
                    City = "testCity",
                    State = "testState",
                    ZipCode = 12345,
                    AddressUuid = new Guid("2BD67540-3D24-4904-A35D-D2B9F7BE3883"),
                    Id = 1,
                    IsVerified = true
                }
            };
        }

        public List<PatientNotes> GetPatientNotes()
        {
            return new List<PatientNotes>()
            {
                new PatientNotes()
                {
                    Note = "Test Note" ,
                    CreatedByUserId = 1,
                    CreatedDateTime = new DateTime(2020,1,1),
                }
            };
        }
    }
}
