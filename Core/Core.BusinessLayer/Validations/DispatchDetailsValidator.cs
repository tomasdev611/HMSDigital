using FluentValidation;
using HMSDigital.Core.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class DispatchDetailsValidator : AbstractValidator<DispatchInstructionRequest>
    {
        public DispatchDetailsValidator()
        {
            RuleFor(d => d.OrderHeaderId).GreaterThan(0)
               .When(d => d.TransferRequestId == null)
               .WithMessage(d => $"Either Order Id or transferRequestId is required");

            RuleFor(d => d.TransferRequestId).GreaterThan(0)
               .When(d => d.OrderHeaderId == null)
               .WithMessage(d => $"Either Order Id or transferRequestId is required");

            RuleFor(d => d.OrderHeaderId).Null()
               .When(d => d.TransferRequestId != null)
               .WithMessage(d => $"Order Id cannot be provided with transfer request Id");

            RuleFor(d=> d.DispatchEndDateTime).NotEmpty()
                .WithMessage(d => $"Dispatch end date time is required");

            RuleFor(d => d.DispatchStartDateTime).NotEmpty()
                .WithMessage(d => $"Dispatch start date time is required");
        }
    }
}
