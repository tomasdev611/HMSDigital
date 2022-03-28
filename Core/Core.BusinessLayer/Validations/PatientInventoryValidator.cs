using FluentValidation;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class PatientInventoryValidator : AbstractValidator<ViewModels.PatientInventoryRequest>
    {
        public PatientInventoryValidator(bool isSerialized, bool isAssetTagged, bool isLotNumbered)
        {
            RuleFor(i => i.SerialNumber).Empty()
                .When(i => !isSerialized)
                .WithMessage(a => $"Non-Serialized items cannot have a serial Number");

            RuleFor(i => i.AssetTagNumber).NotEmpty()
                 .When(i => isAssetTagged)
                 .WithMessage(a => $"Asset tagged items must have a Asset tag Number");

            RuleFor(i => i.AssetTagNumber).Empty()
                .When(i => !isAssetTagged)
                .WithMessage(a => $"Non-AssetTagged items cannot have a AssetTag Number");

            RuleFor(i => i.LotNumber).NotEmpty()
                .When(i => isLotNumbered)
                .WithMessage(a => $"Lot numbered items must have a Lot Number");

            RuleFor(i => i.LotNumber).Empty()
                .When(i => !isLotNumbered)
                .WithMessage(a => $"Non-LotNumbered items cannot have a Lot Number");

            RuleFor(i => i.Quantity).Equal(1)
                .When(i => isSerialized || isAssetTagged)
                .WithMessage(a => $"Quantity should be 1 for serialized/assetTagged items");

            RuleFor(i => i.HospiceId).NotEmpty()
                .WithMessage(a => $"Hospice Id cannot be empty");

            RuleFor(i => i.HospiceLocationId).NotEmpty()
                .WithMessage(a => $"Hospice Location Id cannot be empty");
        }
    }
}
