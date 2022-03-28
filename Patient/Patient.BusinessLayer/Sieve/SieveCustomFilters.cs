using HMSDigital.Patient.Data.Models;
using Sieve.Services;
using System.Linq;

namespace HMSDigital.Patient.BusinessLayer.Sieve
{
    public class SieveCustomFilters : ISieveCustomFilterMethods
    {
        public IQueryable<PatientDetails> IsPending(IQueryable<PatientDetails> source, string op, string[] values)
        {
            if (values != null && bool.TryParse(values[0], out var isPending) && isPending)
            {
                return source.Where(p => p.StatusId == (int)Data.Enums.PatientStatusTypes.Pending);
            }

            return source;
        }
    }
}
