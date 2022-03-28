using AutoMapper;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class OrderLineItemsService : IOrderLineItemsService
    {
        private readonly IOrderLineItemsRepository _orderLineItemsRepository;

        private readonly IMapper _mapper;

        public OrderLineItemsService(IOrderLineItemsRepository orderLineItemsRepository,
            IMapper mapper)
        {
            _orderLineItemsRepository = orderLineItemsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderLineItem>> GetAllOrderLineItems()
        {
            var orderLineItemModels = await _orderLineItemsRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderLineItem>>(orderLineItemModels);
        }

        public async Task<OrderLineItem> GetOrderLineItemById(int orderLineItemId)
        {
            var orderLineItemModel = await _orderLineItemsRepository.GetByIdAsync(orderLineItemId);
            return _mapper.Map<OrderLineItem>(orderLineItemModel);
        }
    }
}
