using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class OrderLineItemStatusValidator : AbstractValidator<Data.Models.OrderLineItems>
    {
        public OrderLineItemStatusValidator(IEnumerable<Data.Models.OrderFulfillmentLineItems> orderFulfillmentLineItems,
                                            Data.Models.DispatchInstructions dispatchInstruction)
        {
            RuleFor(l => l.DispatchStatusId)
             .Equal((int)Data.Enums.OrderLineItemStatusTypes.Completed)
             .When(l => GetFulfilledItemCount(l.Id, l.OrderHeaderId, orderFulfillmentLineItems) >= l.ItemCount);

            RuleFor(l => l.DispatchStatusId)
               .Must(ds => ds == (int)Data.Enums.OrderLineItemStatusTypes.Scheduled || ds == (int)Data.Enums.OrderLineItemStatusTypes.OnTruck
                                || ds == (int)Data.Enums.OrderLineItemStatusTypes.PreLoad || ds == (int)Data.Enums.OrderLineItemStatusTypes.Loading_Truck
                                || ds == (int)Data.Enums.OrderLineItemStatusTypes.OutForFulfillment
                                || ds == (int)Data.Enums.OrderLineItemStatusTypes.Enroute || ds == (int)Data.Enums.OrderLineItemStatusTypes.OnSite
                                || ds == (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment || ds == (int)Data.Enums.OrderLineItemStatusTypes.Completed)
               .When(l => dispatchInstruction != null);

            RuleFor(l => l.DispatchStatusId)
               .Equal((int)Data.Enums.OrderLineItemStatusTypes.Planned)
               .When(l => dispatchInstruction == null
                        && (l.DispatchStatusId != (int)Data.Enums.OrderLineItemStatusTypes.Completed || l.DispatchStatusId != (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment)
                        && GetFulfilledItemCount(l.Id, l.OrderHeaderId, orderFulfillmentLineItems) == 0);



            RuleFor(l => l.StatusId)
                .Equal((int)Data.Enums.OrderLineItemStatusTypes.Completed)
                .When(l => GetFulfilledItemCount(l.Id, l.OrderHeaderId, orderFulfillmentLineItems) >= l.ItemCount);


            RuleFor(o => o.StatusId)
              .Must(s => s == (int)Data.Enums.OrderLineItemStatusTypes.Scheduled || s == (int)Data.Enums.OrderLineItemStatusTypes.OnTruck
                               || s == (int)Data.Enums.OrderLineItemStatusTypes.OutForFulfillment
                               || s == (int)Data.Enums.OrderLineItemStatusTypes.Enroute || s == (int)Data.Enums.OrderLineItemStatusTypes.OnSite
                               || s == (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment || s == (int)Data.Enums.OrderLineItemStatusTypes.Completed)
              .When(o => dispatchInstruction != null);


            RuleFor(o => o.StatusId)
               .Equal((int)Data.Enums.OrderLineItemStatusTypes.Planned)
               .When(l => dispatchInstruction == null
                        && (l.DispatchStatusId != (int)Data.Enums.OrderLineItemStatusTypes.Completed || l.DispatchStatusId != (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment)
                        && GetFulfilledItemCount(l.Id, l.OrderHeaderId, orderFulfillmentLineItems) == 0);
        }

        private int GetFulfilledItemCount(int lineItemId, int orderId, IEnumerable<Data.Models.OrderFulfillmentLineItems> orderFulfillmentLineItems)
        {
            var count = 0;
            var fulfilledLineItems = orderFulfillmentLineItems.Where(l => l.OrderLineItemId == lineItemId
                                                                      && l.OrderHeaderId == orderId);
            if (fulfilledLineItems != null && fulfilledLineItems.Count() > 0)
            {
                count = fulfilledLineItems.Where(o => o.Quantity.HasValue).Sum(f => f.Quantity.Value);
            }
            return count;
        }
    }
}
