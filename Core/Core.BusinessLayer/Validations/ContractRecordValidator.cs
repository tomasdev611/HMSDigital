using FluentValidation;
using CommonValidation = HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ContractRecordValidator : AbstractValidator<NSContractRecordRequest>
    {
        public ContractRecordValidator()
        {
            RuleFor(c => c.NetSuiteContractRecordId).NotEmpty()
               .WithMessage(a => $"NetSuiteContractRecordId is required");

            RuleFor(c => c.CreatedByUserEmail).NotEmpty()
               .WithMessage(a => $"CreatedByUserEmail is required");

            RuleFor(c => c.EffectiveStartDate).NotEmpty()
               .WithMessage(a => $"EffectiveStartDate is required");

            RuleFor(c => c.EffectiveEndDate).NotEmpty()
               .WithMessage(a => $"EffectiveEndDate is required");
        }
    }
}
