using FluentValidation;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class FulfillmentValidator : AbstractValidator<OrderFulfillmentRequest>
    {
        public FulfillmentValidator()
        {
            RuleFor(d => d.OrderId).GreaterThan(0)
               .WithMessage(a => $"Order Id is not valid");

            RuleFor(d => d.FulfillmentStartAtLatitude).GreaterThan(-90)
               .WithMessage(a => $"Latitude should be greater than -90 degree")
               .LessThan(90)
               .WithMessage(a => $"Latitude should be Less than 90 degree");

            RuleFor(d => d.FulfillmentStartAtLongitude).GreaterThan(-180)
                .WithMessage(a => $"Longitude should be greater than -180 degree")
                .LessThan(180)
                .WithMessage(a => $"Longitude should be Less than 180 degree");

            RuleFor(d => d.FulfillmentItems).NotNull()
                .WithMessage(d => $"Inventory Items should be provided for a fulfillment request");

            RuleForEach(d => d.FulfillmentItems)
            .SetValidator(new FulfillmentItemsValidator());
        }
    }
}
