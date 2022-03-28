using FluentValidation;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class AddOnsGroupValidator : AbstractValidator<AddOnsConfigRequest>
    {
        public AddOnsGroupValidator()
        {
            RuleFor(d => d.Name).NotEmpty()
               .WithMessage(a => $"Addons group name is not valid");


            RuleFor(d => d.ProductIds).NotEmpty()
                .WithMessage(d => $"Products/items should be provided for a addons group");
        }
    }
}
