using FluentValidation;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class PhoneNumberValidator : AbstractValidator<AddressRequest>
    {
        public PhoneNumberValidator()
        {
            RuleFor(p => p.Phone)
               .Matches(@"^0|\d{10}$")
               .When(p => !string.IsNullOrEmpty(p.Phone))
               .WithMessage(a => $"phone number must be of 10 digit");
        }
    }
}
