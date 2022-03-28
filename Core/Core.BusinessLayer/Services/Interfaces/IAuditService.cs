using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IAuditService
    {
        Task<AzureLogResponse<ViewModels.AuditLog>> GetAuditLogs(APILogRequest apiLogRequest);

        Task<PaginatedList<ViewModels.AuditLog>> GetAlldispatchUpdateAuditLogs(SieveModel sieveModel);

        Task<IEnumerable<ViewModels.UserAuditCsvReport>> GetAllUsersAuditAsCsvReport(SieveModel SieveModel);

        Task AddUsersAudit(ViewModels.AuditLog userAuditLog);
    }
}
