using HMSDigital.Core.ViewModels;
using HMSDigital.Patient.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IFinanceService
    {
        Task FixPatientHospice(Guid patientUUID, FixPatientHospiceRequest fixPatientHospiceRequest);

        Task MergePatient(Guid patientUUID, MergePatientBaseRequest mergePatientRequest);
        
        Task MovePatientToHospiceLocation(Guid patientUUID, MovePatientToHospiceLocationRequest movePatientToHospiceLocationRequest);
    }
}
