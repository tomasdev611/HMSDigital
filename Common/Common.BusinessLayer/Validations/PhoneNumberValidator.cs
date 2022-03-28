using FluentValidation;
using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.BusinessLayer.Validations
{
    public class PhoneNumberValidator : AbstractValidator<PhoneNumberMinimal>
    {
        public PhoneNumberValidator()
        {
            RuleFor(u => u.Number.ToString()).NotEmpty()
               .WithMessage(a => $"Phone Number must be provided")
               .Matches(@"^\d{10}$")
               .WithMessage(a => $"phone number must be of 10 digit");
        }
    }
}
