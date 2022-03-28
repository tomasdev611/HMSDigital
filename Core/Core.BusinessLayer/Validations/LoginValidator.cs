using FluentValidation;
using HMSDigital.Core.BusinessLayer.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class LoginValidator : AbstractValidator<ViewModels.LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(u => u.Password).NotEmpty()
                .When(u => u.GrantType == GrantTypes.PASSWORD)
                .WithMessage(a => $"Password should not be null");

            RuleFor(u => u.Username).NotEmpty()
                .When(u => u.GrantType == GrantTypes.PASSWORD)
                .WithMessage(a => $"Login Email Address should not be null")
                .EmailAddress()
                .When(u => u.GrantType == GrantTypes.PASSWORD)
                .WithMessage(a => $"Invalid Email Address");

            RuleFor(u => u.RefreshToken).NotEmpty()
                .When(u => u.GrantType == GrantTypes.REFRESH_TOKEN)
                .WithMessage(a => $"Refresh token should not be null");

            RuleFor(u => u.GrantType).NotEmpty()
                .WithMessage(a => $"Grant type should not be null");
        }
    }
}
