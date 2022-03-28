using FluentValidation;
using CommonValidation = HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class FacilityPhoneNumberValidator : AbstractValidator<FacilityPhoneNumber>
    {
        public FacilityPhoneNumberValidator()
        {
            RuleFor(fa => fa.PhoneNumber)
              .SetValidator(new CommonValidation.PhoneNumberValidator());
        }
    }
}
