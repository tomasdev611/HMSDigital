using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public class TransferRequestsService : ITransferRequestsService
    {
        private readonly IItemTransferRequestRepository _itemTransferRequestRepository;

        private readonly IPaginationService _paginationService;

        private readonly IMapper _mapper;

        public TransferRequestsService(IItemTransferRequestRepository itemTransferRequestRepository,
            IPaginationService paginationService,
            IMapper mapper)
        {
            _itemTransferRequestRepository = itemTransferRequestRepository;
            _paginationService = paginationService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ItemTransferRequest>> GetAllTransferRequests(SieveModel sieveModel)
        {
            _itemTransferRequestRepository.SieveModel = sieveModel;
            var transferRequestModels = await _itemTransferRequestRepository.GetAllAsync();
            var totalRecords = await _itemTransferRequestRepository.GetCountAsync(h => true);
            var transferRequests = _mapper.Map<IEnumerable<ItemTransferRequest>>(transferRequestModels);
            return _paginationService.GetPaginatedList(transferRequests, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<ItemTransferRequest> GetTransferRequestById(int transferRequestId)
        {
            var transferRequest = await _itemTransferRequestRepository.GetByIdAsync(transferRequestId);
            return _mapper.Map<ItemTransferRequest>(transferRequest);
        }
    }
}
