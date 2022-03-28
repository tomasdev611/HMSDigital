using FluentValidation;
using CommonValidation = HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class FacilityBulkUploadValidator : AbstractValidator<FacilityCsvRequest>
    {
        public FacilityBulkUploadValidator()
        {
            RuleFor(f => f.Name).NotEmpty()
               .WithMessage(a => $"facility Name should not be empty");

            RuleFor(f => f.HospiceLocationName).NotEmpty()
               .WithMessage(a => $"Hospice location Name should not be empty");

            RuleFor(f => f.ZipCode.ToString()).NotEmpty()
                .WithMessage(f => $"Zip Code should not be null")
                .Matches(@"^\d{5}(?:[-\s]\d{4})?$")
                .WithMessage(f => $"A valid zip code must be provided");

            RuleFor(f => f.AddressLine1).NotEmpty()
               .WithMessage(f => $"Address Line 1 must be provided");

            RuleFor(f => f.City).NotEmpty()
               .WithMessage(f => $"City must be provided");

            RuleFor(f => f.State).NotEmpty()
               .WithMessage(f => $"State must be provided");

            RuleFor(u => u.PhoneNumber.ToString()).NotEmpty()
               .WithMessage(a => $"Phone Number must be provided")
               .Matches(@"^\d{10}$")
               .WithMessage(a => $"phone number must be of 10 digit");
        }
    }
}
