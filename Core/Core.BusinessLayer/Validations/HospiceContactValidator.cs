using FluentValidation;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class HospiceContactValidator : AbstractValidator<HospiceContactRequest>
    {
        public HospiceContactValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty()
                .WithMessage(a => $"FirstName should not be empty")
                .Matches("^[a-zA-Z-'.,/ ]*$")
                .WithMessage(a => $"First Name don't allow digit");

            RuleFor(u => u.LastName).NotEmpty()
                .WithMessage(a => $"LastName should not be empty")
                .Matches("^[a-zA-Z-'.,/ ]*$")
                .WithMessage(a => $"Last Name don't allow digit");

            RuleFor(u => u.Email).NotEmpty()
                .WithMessage(a => $"Email Address should not be empty")
                .EmailAddress()
                .WithMessage(a => $"Invalid Email Address");

            RuleFor(u => u.Phone)
               .Matches(@"^0|\d{10}$")
               .WithMessage(a => $"Phone Number must be of 10 digit");
        }
    }
}
