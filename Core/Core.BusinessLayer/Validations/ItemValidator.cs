using FluentValidation;
using HMSDigital.Common.BusinessLayer.Validations;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ItemValidator : AbstractValidator<ViewModels.ItemRequest>
    {
        public ItemValidator()
        {
            RuleFor(u => u.Name).NotEmpty()
                .WithMessage(a => $"Invalid Item Name");

            RuleFor(u => u.ItemNumber).NotEmpty()
                .WithMessage(a => $"Invalid Item Number");

            RuleFor(pa => pa.Description).NotEmpty()
               .WithMessage(p => $"Invalid Item Description");
        }
    }
}
