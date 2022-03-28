using FluentValidation;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class DispatchValidator : AbstractValidator<DispatchMovementRequest>
    {
        public DispatchValidator()
        {
            RuleFor(d => d.RequestId).GreaterThan(0)
               .WithMessage(a => $"Dispatch Request Id is not valid");

            RuleFor(d => d.Latitude).GreaterThan(-90)
               .WithMessage(a => $"Latitude should be greater than -90 degree")
               .LessThan(90)
               .WithMessage(a => $"Latitude should be Less than 90 degree");

            RuleFor(d => d.Longitude).GreaterThan(-180)
                .WithMessage(a => $"Longitude should be greater than -180 degree")
                .LessThan(180)
                .WithMessage(a => $"Longitude should be Less than 180 degree");

            RuleFor(d => d.DispatchItems).NotNull()
                .WithMessage(d => $"Inventory Items should be provided for a dispatch request");

            RuleForEach(d => d.DispatchItems)
            .SetValidator(new DispatchItemsValidator());
        }
    }
}
