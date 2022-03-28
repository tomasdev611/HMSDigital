using HMSDigital.Report.ViewModels;
using System.Threading.Tasks;
using Sieve.Models;

namespace HMSDigital.Report.BusinessLayer.Interfaces
{
    public interface IReportService
    {
        Task<ReportListResponse> GetOrdersMetric(SieveModel sieveModel, string groupedBy, bool ignoreFilter = false);

        Task<ReportValueResponse> GetPatientsMetric(string filters, bool ignoreFilter = false);
    }
}
