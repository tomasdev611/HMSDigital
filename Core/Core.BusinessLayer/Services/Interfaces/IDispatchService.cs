using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IDispatchService
    {
        Task<SiteLoadList> GetLoadList(int siteId, SieveModel sieveModel);

        Task PickupDispatchRequest(DispatchMovementRequest pickupRequest);

        Task DropDispatchRequest(DispatchMovementRequest completeRequest);

        Task<IEnumerable<HospiceSource.Digital.NetSuite.SDK.ViewModels.DispatchRecordUpdateResponse>> UpdateDispatchRecords(IEnumerable<DispatchRecordUpdateRequest> dispatchRecordUpdateRequests);

        Task<DispatchResponse> GetLoggedInDriverDispatch(SieveModel sieveModel);

        Task<IEnumerable<OrderLocation>> GetCurrentOrderLocation(IEnumerable<int> orderIds);

        Task<PaginatedList<DispatchInstruction>> GetAllDispatchInstructions(SieveModel sieveModel);

        Task<DispatchInstruction> GetDispatchInstructionById(int dispatchInstructionId);

        Task<IEnumerable<DispatchInstruction>> CreateDispatchInstruction(DispatchAssignmentRequest dispatchAssignmentRequest);

        Task UnassignOrder(int orderId);

        Task<Data.Models.OrderHeaders> FixOrderStatus(Data.Models.OrderHeaders orderModel, bool previewChanges);

    }
}
