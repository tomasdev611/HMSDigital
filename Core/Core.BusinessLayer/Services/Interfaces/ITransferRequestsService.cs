using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface ITransferRequestsService
    {
        Task<PaginatedList<ItemTransferRequest>> GetAllTransferRequests(SieveModel sieveModel);

        Task<ItemTransferRequest> GetTransferRequestById(int transferRequestId);
    }
}
