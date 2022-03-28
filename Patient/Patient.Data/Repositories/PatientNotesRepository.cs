using HMSDigital.Patient.Data.Models;
using HMSDigital.Patient.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Patient.Data.Repositories
{
    public class PatientNotesRepository : RepositoryBase<PatientNotes>, IPatientNotesRepository
    {
        public PatientNotesRepository(HMSPatientAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

    }
}
