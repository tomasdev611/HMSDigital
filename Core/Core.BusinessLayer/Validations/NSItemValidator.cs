using FluentValidation;
using System.Linq;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class NSItemValidator : AbstractValidator<ViewModels.NetSuite.NSItemRequest>
    {
        public NSItemValidator()
        {
            RuleFor(u => u.NetSuiteItemId).NotEmpty()
               .WithMessage(a => $"Internal ItemId cannot be empty");

            RuleFor(u => u.Name).NotEmpty()
                .WithMessage(a => $"Invalid Item Name");

            RuleFor(u => u.ItemNumber).NotEmpty()
                .WithMessage(a => $"Invalid Item Number");

            RuleFor(pa => pa.Categories).NotEmpty()
               .WithMessage(p => $"Item Category can not be empty");

            RuleFor(pa => pa.CreatedByUserEmail).NotEmpty()
              .WithMessage(p => $"CreatedByUserEmail cannot be empty");

            RuleFor(pa => pa.IsDME).NotNull()
                .WithMessage(p => $"IsDME cannot be null");

            RuleFor(pa => pa.IsConsumable).NotNull()
                .WithMessage(p => $"IsConsumable cannot be null");

            RuleForEach(i => i.Inventory)
                .SetValidator(i => new NSInventoryValidator(i.IsSerialized, i.IsAssetTagged, i.IsLotNumbered));
        }

    }
}
