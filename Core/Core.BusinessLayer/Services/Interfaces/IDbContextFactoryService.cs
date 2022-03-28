using HMSDigital.Core.Data;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IDbContextFactoryService
    {
        Task<HMSDigitalAuditContext> GetDBContext();
    }
}
