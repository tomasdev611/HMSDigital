using FluentValidation;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ApproverValidator : AbstractValidator<ApproverRequest>
    {
        public ApproverValidator()
        {
            RuleFor(a => a.NetSuiteCustomerId).NotEmpty()
                .WithMessage(a => $"Customer id cannot be empty");

            RuleFor(a => a.Approvers).NotNull()
                .WithMessage(a => $"Approvers cannot be null");
        }
    }
}
