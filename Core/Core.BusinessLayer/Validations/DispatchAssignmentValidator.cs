using FluentValidation;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.ViewModels;
using System;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    class DispatchAssignmentValidator : AbstractValidator<DispatchAssignmentRequest>
    {
        public DispatchAssignmentValidator()
        {

            RuleForEach(d => d.DispatchDetails)
                .SetValidator(new DispatchDetailsValidator());

        }
    }
}
