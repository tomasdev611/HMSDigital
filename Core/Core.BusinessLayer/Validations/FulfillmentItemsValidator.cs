﻿using FluentValidation;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class FulfillmentItemsValidator : AbstractValidator<FulfillmentItem>
    {
        public FulfillmentItemsValidator()
        {
            RuleFor(d => d.AssetTagNumber).NotEmpty()
               .When(d => string.IsNullOrEmpty(d.LotNumber) && string.IsNullOrEmpty(d.SerialNumber) && !d.ItemId.HasValue)
               .WithMessage(d => $"Asset tag number/Serial Number/Lot Number/ItemId is required");
        }
    }
}
