using FluentValidation;
using HMSDigital.Core.ViewModels;
using System;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class WebPortalOrderHeaderValidator : AbstractValidator<OrderHeaderRequest>
    {
        public WebPortalOrderHeaderValidator(bool isNetSuiteOrder)
        {
            RuleForEach(o => o.OrderLineItems)
              .SetValidator(new WebPortalOrderLineItemValidator());

            RuleFor(o => o.HospiceMemberId).NotEmpty()
                .When(l => !isNetSuiteOrder)
                .WithMessage(l => $"Hospice member Id cannot be empty");

            RuleFor(o => o.PatientUuid).NotEmpty()
                .WithMessage(l => $"Patient UUID cannot be empty");

            RuleFor(o => o.OrderTypeId).NotEmpty()
                .WithMessage(l => $"Order type id cannot be empty")
                .Must(OrderTypeId => IsValidOrderTypeId(OrderTypeId))
                .WithMessage(o => $"Order type {o.OrderTypeId} is not valid");

            RuleFor(o => o.OrderStatusId).NotEmpty()
                .WithMessage(l => $"Order status id cannot be empty")
                .Must(orderStatusId => IsValidOrderStatusId(orderStatusId))
                .WithMessage(o => $"Order status {o.OrderStatusId} is not valid");

            RuleFor(o => o.RequestedEndDateTime).NotEqual(System.DateTime.MinValue)
                .WithMessage(l => $"DeliveryDateTo cannot be empty");

            RuleFor(o => o.DeliveryAddress).NotEmpty()
              .When(o => IsDeliveryAddressRequired(o.OrderTypeId))
              .WithMessage(o => $"Delivery Address is required");

            RuleFor(o => o.PickupAddress).NotEmpty()
              .When(o => IsPickupAddressRequired(o.OrderTypeId))
              .WithMessage(o => $"Pickup Address is required");

        }

        private bool IsDeliveryAddressRequired(int orderTypeId)
        {
            if (orderTypeId == (int)Data.Enums.OrderTypes.Delivery
                || orderTypeId == (int)Data.Enums.OrderTypes.Exchange
                || orderTypeId == (int)Data.Enums.OrderTypes.Respite
                || orderTypeId == (int)Data.Enums.OrderTypes.Patient_Move)
            {
                return true;
            }
            return false;
        }

        private bool IsPickupAddressRequired(int orderTypeId)
        {
            if (orderTypeId == (int)Data.Enums.OrderTypes.Patient_Move
                || orderTypeId == (int)Data.Enums.OrderTypes.Pickup)
            {
                return true;
            }
            return false;
        }

        private bool IsValidOrderTypeId(int orderTypeId)
        {
            if (!Enum.IsDefined(typeof(Data.Enums.OrderTypes), orderTypeId))
            {
                return false;
            }
            return true;
        }

        private bool IsValidOrderStatusId(int orderStatusId)
        {
            if (!Enum.IsDefined(typeof(Data.Enums.OrderHeaderStatusTypes), orderStatusId))
            {
                return false;
            }
            return true;
        }

    }
}
