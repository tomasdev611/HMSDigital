using FluentValidation;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ResetPasswordValidator : AbstractValidator<ViewModels.ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(u => u.Email).NotEmpty()
                .WithMessage(a => $"Login Email Address should not be empty")
                .EmailAddress()
                .WithMessage(a => $"Invalid Email Address");

            RuleFor(u => u.Password).NotEmpty()
                .WithMessage(a => $"Password should not be empty");

            RuleFor(u => u.Otp).NotEmpty()
                .WithMessage(a => $"Otp should not be empty");
        }
    }
}
