using FluentValidation;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class NSInventoryValidator : AbstractValidator<ViewModels.NetSuite.NSInventory>
    {
        public NSInventoryValidator(bool isSerialized, bool isAssetTagged, bool isLotNumbered)
        {
            
            RuleFor(i => i.SerialNumber).NotEmpty()
                .When(i => isSerialized)
                .WithMessage(a => $"Serialized items must have a serial Number");

            //RuleFor(i => i.AssetTagNumber).NotEmpty()
            //     .When(i => isAssetTagged)
            //     .WithMessage(a => $"Asset tagged items must have a Asset tag Number");

            RuleFor(i => i.LotNumber).NotEmpty()
                .When(i => isLotNumbered)
                .WithMessage(a => $"Lot numbered items must have a Lot Number");

            RuleFor(i => i.NetSuiteInventoryId).NotEmpty()
                .When(i => isSerialized || isAssetTagged)
                .WithMessage(a => $"Internal Inventory Id is required for serialized/assetTagged items");

            RuleFor(i => i.NetSuiteInventoryId).Empty()
               .When(i => !isSerialized && !isAssetTagged)
               .WithMessage(a => $"Internal Inventory Id should be empty for standalone items");
        }
    }
}
