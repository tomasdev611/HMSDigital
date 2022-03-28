using FluentValidation;
using HMSDigital.Common.BusinessLayer.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.BusinessLayer.Validations
{
    public class PatientValidator : AbstractValidator<ViewModels.PatientCreateRequest>
    {
        public PatientValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty()
                 .WithMessage(p => $"First name should not be empty");

            RuleFor(p => p.LastName).NotEmpty()
                .WithMessage(p => $"Last name should not be empty");

            RuleFor(p => p.PatientWeight).NotEmpty()
                .WithMessage(p => $"Weight should not be null")
                .GreaterThan(0)
                .WithMessage(p => $"Weight should be greater than 0 ");

            RuleFor(p => p.PatientHeight).NotEmpty()
                .WithMessage(p => $"Height should not be null")
                .GreaterThan(0)
                .WithMessage(p => $"Height should be greater than 0 ");

            RuleForEach(p => p.PatientAddress).SetValidator(new PatientAddressValidator());

            RuleFor(p => p.PhoneNumbers).NotEmpty()
                .WithMessage(p => $"Phone number must be provided");

            RuleFor(p => p.HospiceLocationId).NotEmpty()
                .WithMessage(p => $"Hospice location should not be null or empty");

            RuleForEach(p => p.PhoneNumbers)
                .SetValidator(new PhoneNumberValidator());
        }
    }
}
