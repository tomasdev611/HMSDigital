using FluentValidation;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class WebPortalOrderLineItemValidator : AbstractValidator<CoreOrderLineItemRequest>
    {
        public WebPortalOrderLineItemValidator()
        {
            RuleFor(l => l.ItemId).NotEqual(0)
               .WithMessage(l => $"Item Id cannot be empty");

        }
    }
}
