using FluentValidation;
using HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class WarehouseValidator : AbstractValidator<WarehouseRequest>
    {
        public WarehouseValidator()
        {
            RuleFor(w => w.Name).NotEmpty()
                .WithMessage(a => $"Invalid Name");

            RuleFor(w => w.LocationType).NotEmpty()
                .WithMessage(a => $"Location type can not be empty");

            RuleFor(w => w.CreatedByUserEmail).NotEmpty()
                .WithMessage(a => $"CreatedByUserEmail can not be empty");

            RuleFor(w => w.NetSuiteLocationId).NotEmpty()
                .WithMessage(w => $"NetSuiteLocationId is required");

            RuleForEach(w => w.PhoneNumbers)
                .SetValidator(new WarehousePhoneNumberValidator());
        }
    }
}
