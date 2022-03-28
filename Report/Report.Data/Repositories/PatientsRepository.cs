using HMSDigital.Report.Data.Models;
using HMSDigital.Report.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Report.Data.Repositories
{
    public class PatientsRepository : RepositoryBase<PatientsMetric>, IPatientsRepository
    {
        public const string PatientsCountByStatus = "report.sp_PatientsCountByStatus";
        public PatientsRepository(HMSReportAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {
        }

        public async Task<int> GetPatientsByStatus(string filters)
        {
            var paramsQuery = new List<string>();
            if (!string.IsNullOrEmpty(filters))
            {
                var paramsArray = filters.Split(",").Where(x => !string.IsNullOrEmpty(x)).ToList();
                foreach (var item in paramsArray)
                {
                    var spParam = item.Split("==");

                    paramsQuery.Add($"@{spParam[0]}={spParam[1]}");
                }
            }

            var parameters = string.Join(',', paramsQuery);
            var result = await _dbContext.PatientsMetric.FromSqlRaw($"exec {PatientsCountByStatus} {parameters}").ToListAsync();

            return result.FirstOrDefault()?.Total ?? 0;
        }
    }
}
