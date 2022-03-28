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
    public class PatientsRepository : RepositoryBase<PatientDetails>, IPatientsRepository
    {
        public PatientsRepository(HMSPatientAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<PatientDetails>> GetAllAsync()
        {
            var entities = _dbContext.Set<PatientDetails>()
                .Include(p => p.PatientNotes)
                .Include(p => p.PhoneNumbers)
                .Include(p => p.PatientAddress)
                    .ThenInclude(pa => pa.Address)
                .AsQueryable();

            if (string.IsNullOrEmpty(SieveModel?.Sorts))
            {
                entities = entities.OrderByDescending(p => p.UpdatedDateTime);
            }

            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<PatientDetails> GetByIdAsync(int id)
        {
            var entities = _dbContext.Set<PatientDetails>()
                .Include(p => p.PatientAddress)
                    .ThenInclude(pa => pa.Address)
                .Include(p => p.PhoneNumbers)
                .Include(p => p.PatientNotes)
                .Where(c => c.Id == id)
                .AsQueryable();

            var patient = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
            if (patient != null)
            {
                patient.PatientAddress = patient.PatientAddress.OrderBy(p => p.AddressId).ToList();
                patient.PhoneNumbers = patient.PhoneNumbers.OrderBy(ph => ph.Id).ToList();
                patient.PatientNotes = patient.PatientNotes.OrderBy(pn => pn.Id).ToList();
            }
            return patient;
        }

        public override async Task<PatientDetails> GetAsync(Expression<Func<PatientDetails, bool>> where)
        {
            var entities = _dbContext.Set<PatientDetails>()
                .Include(p => p.PatientAddress)
                    .ThenInclude(pa => pa.Address)
                .Include(p => p.PhoneNumbers)
                .Include(p => p.PatientNotes)
                .Include(p => p.Status)
                .Include(p => p.StatusReason)
                .Where(where)
                .AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

        public override async Task<IEnumerable<PatientDetails>> GetManyAsync(Expression<Func<PatientDetails, bool>> where)
        {
            var entities = _dbContext.Set<PatientDetails>()
                .Include(p => p.PhoneNumbers)
                .Include(p => p.PatientNotes)
                .Include(p => p.PatientAddress)
                    .ThenInclude(pa => pa.Address)
                .Include(p => p.Status)
                .Include(p => p.StatusReason)
                .Where(where)
                .AsQueryable();

            if (string.IsNullOrEmpty(SieveModel?.Sorts))
            {
                entities = entities.OrderByDescending(p => p.UpdatedDateTime);
            }

            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
