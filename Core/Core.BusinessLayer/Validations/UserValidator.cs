using FluentValidation;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class UserValidator : AbstractValidator<ViewModels.UserMinimal>
    {
        public UserValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty()
                 .WithMessage(p => $"First name should not be empty");

            RuleFor(p => p.LastName).NotEmpty()
                .WithMessage(p => $"Last name should not be empty");

            RuleFor(u => u.Email).NotEmpty()
                .WithMessage(a => $"Login Email Address should not be null")
                .EmailAddress()
                .WithMessage(a => $"Invalid Email Address");

            RuleFor(u => u.PhoneNumber.ToString())
               .Matches(@"^0|\d{10}$")
               .WithMessage(a => $"Phone Number must be of 10 digit");
        }
    }
}
