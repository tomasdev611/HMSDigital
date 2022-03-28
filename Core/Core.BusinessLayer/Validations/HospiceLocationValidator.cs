using FluentValidation;
using HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class HospiceLocationValidator : AbstractValidator<HospiceLocationCreateRequest>
    {
        public HospiceLocationValidator()
        {
            RuleFor(l => l.Name).NotEmpty()
               .WithMessage(l => $"Customer location name cannot be empty");

            RuleFor(l => l.Address)
              .SetValidator(new AddressValidator());

            RuleFor(f => f.Address)
             .SetValidator(new PhoneNumberValidator());
        }
    }
}
