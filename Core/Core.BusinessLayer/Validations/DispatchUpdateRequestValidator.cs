using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class DispatchUpdateRequestValidator : AbstractValidator<ViewModels.DispatchRecordUpdateRequest>
    {
        public DispatchUpdateRequestValidator()
        {
            RuleFor(u => u.HmsDeliveryDate)
                .Must(d => d != DateTime.MinValue)
                .WithMessage(a => $"Delievery date should be valid");

            RuleFor(u => u.HmsPickupRequestDate)
                .Must(d => d != DateTime.MinValue)
                .WithMessage(a => $"Pickup request date should be valid");

            RuleFor(u => u.PickupDate)
                .Must(d => d != DateTime.MinValue)
                .WithMessage(a => $"Pickup date should be valid");

            RuleFor(u => u)
                .Must(u => !(u.HmsDeliveryDate == null && u.HmsPickupRequestDate == null && u.PickupDate == null))
                .WithMessage(e => $"All Values cannot be null");
        }

    }
}
