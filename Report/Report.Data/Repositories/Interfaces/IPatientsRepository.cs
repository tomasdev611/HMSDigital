using HMSDigital.Report.Data.Models;
using System.Threading.Tasks;

namespace HMSDigital.Report.Data.Repositories.Interfaces
{
    public interface IPatientsRepository : IRepository<PatientsMetric>
    {
        Task<int> GetPatientsByStatus(string filters);
    }
}
