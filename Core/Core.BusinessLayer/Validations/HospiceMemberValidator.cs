using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class HospiceMemberValidator : AbstractValidator<ViewModels.HospiceMemberCsvRequest>
    {
        public HospiceMemberValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty()
                .WithMessage(a => $"Invalid FirstName")
                .Matches("^[a-zA-Z-'.,/ ]*$")
                .WithMessage(a => $"First Name don't allow digit");

            RuleFor(u => u.LastName).NotEmpty()
                .WithMessage(a => $"Invalid LastName")
                .Matches("^[a-zA-Z-'.,/ ]*$")
                .WithMessage(a => $"Last Name don't allow digit");

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
