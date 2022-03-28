using FluentValidation;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class OtpValidator : AbstractValidator<ViewModels.OtpRequest>
    {
        public OtpValidator()
        {
            RuleFor(u => u.Email).NotEmpty()
                .WithMessage(a => $"Login Email Address should not be empty")
                .EmailAddress()
                .WithMessage(a => $"Invalid Email Address");
        }
    }
}
