using HMSDigital.Core.Data.Models;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories.Interfaces
{
    public interface ISitesRepository : IRepository<Sites>
    {
        Task RefreshSitesCache();
    }
}
