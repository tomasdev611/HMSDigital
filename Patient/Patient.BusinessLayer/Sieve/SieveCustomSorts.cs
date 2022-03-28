using HMSDigital.Patient.Data.Models;
using Sieve.Services;
using System.Linq;

namespace HMSDigital.Patient.BusinessLayer.Sieve
{
    public class SieveCustomSorts : ISieveCustomSortMethods
    {
        public IQueryable<PatientDetails> StatusRelevance(IQueryable<PatientDetails> patients, bool useThenBy, bool desc)
        {
            IQueryable<PatientDetails> result;

            if (useThenBy)
            {
                result = desc ? ((IOrderedQueryable<PatientDetails>)patients)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Active)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Pending)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.PendingActive)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Blank)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Inactive) :
                                ((IOrderedQueryable<PatientDetails>)patients)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Active)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Pending)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.PendingActive)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Blank)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Inactive);
                return result;
            }

            result = desc ? patients.OrderBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Active)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Pending)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.PendingActive)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Blank)
                                    .ThenBy(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Inactive) :
                            patients.OrderByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Active)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Pending)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.PendingActive)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Blank)
                                    .ThenByDescending(p => p.StatusId.HasValue && p.StatusId == (int)Data.Enums.PatientStatusTypes.Inactive);
            return result;
        }
    }
}
