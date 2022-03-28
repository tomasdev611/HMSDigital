using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ChangeUserPasswordValidator : AbstractValidator<ViewModels.ChangePasswordRequest>
    {
        public ChangeUserPasswordValidator()
        {
            RuleFor(u => u.OldPassword).NotEmpty()
                .WithMessage(a => $"Current password is required");

            RuleFor(u => u.NewPassword).NotEmpty()
                .WithMessage(a => $"New password is required");
        }
    }
}
