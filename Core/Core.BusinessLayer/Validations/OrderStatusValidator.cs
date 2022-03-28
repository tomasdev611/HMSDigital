using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class OrderStatusValidator : AbstractValidator<Data.Models.OrderHeaders>
    {
        public OrderStatusValidator(IEnumerable<Data.Models.OrderFulfillmentLineItems> orderFulfillmentLineItems,
                                    IEnumerable<Data.Models.DispatchInstructions> dispatchInstructions)
        {
            RuleFor(o => o.DispatchStatusId)
                .Equal((int)Data.Enums.OrderHeaderStatusTypes.Cancelled)
                .When(o => o.OrderLineItems.Count() == 0);

            RuleFor(o => o.DispatchStatusId)
                .Equal((int)Data.Enums.OrderHeaderStatusTypes.Completed)
                .When(o => o.OrderLineItems.Count() != 0 && o.OrderLineItems.All(l => l.DispatchStatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed));

            RuleFor(o => o.DispatchStatusId)
               .Must(ds => ds == (int)Data.Enums.OrderHeaderStatusTypes.Scheduled || ds == (int)Data.Enums.OrderHeaderStatusTypes.OnTruck
                                || ds == (int)Data.Enums.OrderHeaderStatusTypes.PreLoad || ds == (int)Data.Enums.OrderHeaderStatusTypes.Loading_Truck
                                || ds == (int)Data.Enums.OrderHeaderStatusTypes.OutForFulfillment
                                || ds == (int)Data.Enums.OrderHeaderStatusTypes.Enroute || ds == (int)Data.Enums.OrderHeaderStatusTypes.OnSite
                                || ds == (int)Data.Enums.OrderHeaderStatusTypes.Partial_Fulfillment || ds == (int)Data.Enums.OrderHeaderStatusTypes.Completed)
               .When(o => GetDispatchInstructions(o.Id, dispatchInstructions) != null);

            RuleFor(o => o.DispatchStatusId)
               .Equal((int)Data.Enums.OrderHeaderStatusTypes.Planned)
               .When(o => GetDispatchInstructions(o.Id, dispatchInstructions) == null
                        && (o.DispatchStatusId != (int)Data.Enums.OrderHeaderStatusTypes.Completed || o.DispatchStatusId != (int)Data.Enums.OrderHeaderStatusTypes.Partial_Fulfillment)
                        && o.OrderLineItems.Count() != 0
                        && !o.OrderLineItems.Any(l => l.DispatchStatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed
                                                    || l.DispatchStatusId == (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment));

            RuleFor(o => o.DispatchStatusId)
               .Must(ds => ds != (int)Data.Enums.OrderHeaderStatusTypes.Completed)
               .When(o => o.OrderLineItems.Any() && o.OrderLineItems.Any(l => l.DispatchStatusId != (int)Data.Enums.OrderLineItemStatusTypes.Completed));

            RuleFor(o => o.StatusId)
                .Equal((int)Data.Enums.OrderHeaderStatusTypes.Cancelled)
                .When(o => o.OrderLineItems.Count() == 0);

            RuleFor(o => o.StatusId)
                .Equal((int)Data.Enums.OrderHeaderStatusTypes.Completed)
                .When(o => o.OrderLineItems.Count() != 0 && o.OrderLineItems.All(l => l.StatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed));


            RuleFor(o => o.StatusId)
              .Must(s => s == (int)Data.Enums.OrderHeaderStatusTypes.Scheduled || s == (int)Data.Enums.OrderHeaderStatusTypes.OnTruck
                               || s == (int)Data.Enums.OrderHeaderStatusTypes.OutForFulfillment
                               || s == (int)Data.Enums.OrderHeaderStatusTypes.Enroute || s == (int)Data.Enums.OrderHeaderStatusTypes.OnSite
                               || s == (int)Data.Enums.OrderHeaderStatusTypes.Partial_Fulfillment || s == (int)Data.Enums.OrderHeaderStatusTypes.Completed)
              .When(o => GetDispatchInstructions(o.Id, dispatchInstructions) != null);


            RuleFor(o => o.StatusId)
               .Equal((int)Data.Enums.OrderHeaderStatusTypes.Planned)
               .When(o => GetDispatchInstructions(o.Id, dispatchInstructions) == null
                        && (o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Completed || o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Partial_Fulfillment)
                        && o.OrderLineItems.Count() != 0
                        && !o.OrderLineItems.Any(l => l.StatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed
                                                    || l.StatusId == (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment));

            RuleForEach(o => o.OrderLineItems)
                .SetValidator(ol => new OrderLineItemStatusValidator(orderFulfillmentLineItems, GetDispatchInstructions(ol.Id, dispatchInstructions)));
        }

        private Data.Models.DispatchInstructions GetDispatchInstructions(int orderId, IEnumerable<Data.Models.DispatchInstructions> dispatchInstructions)
        {
            if (dispatchInstructions != null)
            {
                return dispatchInstructions.FirstOrDefault(d => d.OrderHeaderId == orderId);
            }
            return null;
        }
    }
}
