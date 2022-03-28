using FluentValidation;
using HMSDigital.Common.BusinessLayer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class UserSetPasswordValidator : AbstractValidator<ViewModels.NotificationBase>
    {
        public UserSetPasswordValidator()
        {
            RuleFor(u => u.Email).NotEmpty()
                .When(u => u.Channels.Any(c => c == Channels.EMAIL))
                .WithMessage(a => $"Email Address should not be null")
                .EmailAddress()
                .When(u => u.Channels.Any(c => c == Channels.EMAIL))
                .WithMessage(a => $"Invalid Email Address");

            RuleFor(u => u.PhoneNumber.ToString()).NotEmpty()
               .When(u => u.Channels.Any(c => c == Channels.SMS))
               .WithMessage(a => $"Phone Number must be provided")
               .Matches(@"^\d{10}$")
               .When(u => u.Channels.Any(c => c == Channels.SMS))
               .WithMessage(a => $"phone number must be of 10 digit");

        }
    }
}
