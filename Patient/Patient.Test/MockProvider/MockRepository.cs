using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Patient.Data.Models;
using HMSDigital.Patient.Data.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Patient.Test.MockProvider
{
    public class MockRepository
    {
        private readonly MockModels _mockModels;

        public MockRepository(MockModels mockModels)
        {
            _mockModels = mockModels;
        }


        public IUsersRepository GetUsersRepository()
        {
            var usersRepository = new Mock<IUsersRepository>();
            usersRepository.Setup(x => x.GetHospiceAccessByUserId(It.IsAny<int>())).Returns(Task.FromResult(new List<string>() { "*" }.AsEnumerable()));
            return usersRepository.Object;
        }

        public IAddressesRepository GetAddressesRepository()
        {
            var addressesRepository = new Mock<IAddressesRepository>();
            Addresses addresses = null;
            
            addressesRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Addresses, bool>>>()))
                        .Callback<Expression<Func<Addresses, bool>>>(
                          expression =>
                          {
                              var func = expression.Compile();
                              addresses = _mockModels.Addresses.FirstOrDefault(c => func(c));
                          })
                        .Returns(() => Task.FromResult(addresses));

            addressesRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns<int>(id => Task.FromResult(
                                                                               id > 0 ? _mockModels.Addresses.Where(i => i.Id == id).FirstOrDefault() : null));
            return addressesRepository.Object;
        }

        public IPatientsRepository GetPatientsRepository()
        {
            PatientDetails patientDetails = null;
            var patientsRepository = new Mock<IPatientsRepository>();
            patientsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns<int>(id => Task.FromResult(
                                                                                id > 0 ? _mockModels.Patients.Where(i => i.Id == id).FirstOrDefault() : null));
            patientsRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(_mockModels.Patients));

            patientsRepository.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<PatientDetails, bool>>>()))
             .Callback<Expression<Func<PatientDetails, bool>>>(
               expression =>
               {
                   var func = expression.Compile();
                   _mockModels.Patients = _mockModels.Patients.Where(c => func(c)).ToList();
               })
             .Returns(() => Task.FromResult(_mockModels.Patients));

            patientsRepository.Setup(x => x.AddAsync(It.IsAny<PatientDetails>()))
                .Callback<PatientDetails>(
                patientModel =>
                {
                    patientModel.Id = new Random().Next(100000, 1000000);
                    var list = _mockModels.Patients.ToList();
                    list.Add(patientModel);
                    _mockModels.Patients = list;
                })
                .Returns<PatientDetails>(patient => Task.FromResult(patient));

            patientsRepository.Setup(x => x.UpdateAsync(It.IsAny<PatientDetails>()))
                .Callback<PatientDetails>(
                patientModel =>
                {
                    var list = _mockModels.Patients.ToList();
                    var oldPatient = list.Where(c => c.Id == patientModel.Id).FirstOrDefault();
                    oldPatient = patientModel;
                    _mockModels.Patients = list;
                })
                .Returns<PatientDetails>(patient => Task.FromResult(patient));

            patientsRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PatientDetails, bool>>>()))
                         .Callback<Expression<Func<PatientDetails, bool>>>(
                           expression =>
                           {
                               var func = expression.Compile();
                               patientDetails = _mockModels.Patients.FirstOrDefault(c => func(c));
                           })
                         .Returns(() => Task.FromResult(patientDetails));

            return patientsRepository.Object;
        }

        public IPatientNotesRepository GetPatientNotesRepository()
        {
            var patientNotesRepository = new Mock<IPatientNotesRepository>();
            return patientNotesRepository.Object;
        }

        public IPatientAddressRepository GetPatientAddressRepository()
        {
            var patientAddressRepository = new Mock<IPatientAddressRepository>();
            return patientAddressRepository.Object;
        }

        public IHospiceRepository GetHospiceRepository()
        {
            var hospiceRepository = new Mock<IHospiceRepository>();
            return hospiceRepository.Object;
        }

        public IPatientMergeHistoryRepository GetPatientMergeHistoryRepository()
        {
            var patientMergeHistoryRepository = new Mock<IPatientMergeHistoryRepository>();
            return patientMergeHistoryRepository.Object;
        }
    }
}
