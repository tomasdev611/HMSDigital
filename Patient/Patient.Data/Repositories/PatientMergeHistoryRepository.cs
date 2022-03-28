using HMSDigital.Patient.Data.Models;
using HMSDigital.Patient.Data.Repositories.Interfaces;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.Data.Repositories
{
    public class PatientMergeHistoryRepository : RepositoryBase<PatientMergeHistory>, IPatientMergeHistoryRepository
    {
        public PatientMergeHistoryRepository(HMSPatientAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
    }
}
