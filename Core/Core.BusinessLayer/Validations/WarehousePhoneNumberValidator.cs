using FluentValidation;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class WarehousePhoneNumberValidator : AbstractValidator<PhoneNumberReqeust>
    {
        public WarehousePhoneNumberValidator()
        {
            RuleFor(p => p.Number.ToString())
               .Matches(@"^0|\d{10}$")
               .When(p => !string.IsNullOrEmpty(p.Number.ToString()))
               .WithMessage(a => $"phone number must be of 10 digit");
        }
    }
}
