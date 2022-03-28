using FluentValidation;
using CommonValidation = HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class FacilityValidator : AbstractValidator<FacilityRequest>
    {
        public FacilityValidator()
        {
            RuleFor(f => f.Name).NotEmpty()
               .WithMessage(a => $"facility Name should not be null");

            RuleFor(f => f.Address)
              .SetValidator(new CommonValidation.AddressValidator());

            RuleForEach(f => f.FacilityPhoneNumber)
                .SetValidator(new FacilityPhoneNumberValidator());
        }
    }
}
