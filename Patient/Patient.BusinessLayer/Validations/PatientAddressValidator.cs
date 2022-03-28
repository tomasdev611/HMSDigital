using FluentValidation;
using HMSDigital.Common.BusinessLayer.Validations;

namespace HMSDigital.Patient.BusinessLayer.Validations
{
    public class PatientAddressValidator : AbstractValidator<ViewModels.PatientAddressRequest>
    {
        public PatientAddressValidator()
        {
            RuleFor(pa => pa.AddressTypeId).NotEmpty()
               .WithMessage(p => $"Address type should not be null");

            RuleFor(pa => pa.Address).NotEmpty()
               .WithMessage(p => $"Address should not be null")
               .SetValidator(new AddressValidator());
        }
    }
}
