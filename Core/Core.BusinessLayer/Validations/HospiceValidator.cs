using FluentValidation;
using HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class HospiceValidator : AbstractValidator<NSHospiceRequest>
    {
        public HospiceValidator()
        {
            RuleFor(c => c.Name).NotEmpty()
               .WithMessage(a => $"Customer name cannot be empty");

            RuleFor(o => o.CreatedByUserEmail).NotEmpty()
                .WithMessage(l => $"CreatedByUserEmail cannot be empty");

            RuleForEach(c => c.Locations)
                .SetValidator(new HospiceLocationValidator());

            RuleFor(l => l.Address)
             .SetValidator(new AddressValidator());

            RuleFor(f => f.Address)
             .SetValidator(new PhoneNumberValidator());
        }
    }
}
