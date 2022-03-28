using FluentValidation;
using HMSDigital.Common.BusinessLayer.Validations;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class InventoryValidator : AbstractValidator<ViewModels.InventoryRequest>
    {
        public InventoryValidator()
        {
            RuleFor(i => i.CurrentLocationId).GreaterThan(0)
                .WithMessage(a => $"Current Location is required");

            RuleFor(i => i.ItemId).GreaterThan(0)
               .WithMessage(p => $"Item is not specified");

            RuleFor(i=> i.QuantityAvailable).GreaterThanOrEqualTo(0)
                .WithMessage(p => $"Invalid item count ({p.QuantityAvailable})")
                .LessThanOrEqualTo(1)
                .When(i=> !string.IsNullOrEmpty( i.SerialNumber))
                .WithMessage(p=> $"Count can not be more than 1 when Serial Number is specified");
        }
    }
}
