using FluentValidation;
using HMSDigital.Patient.Data.Enums;
using HospiceSource.Digital.Patient.SDK.ViewModels;

namespace HMSDigital.Patient.BusinessLayer.Validations
{
    public class PatientStatusValidator : AbstractValidator<PatientStatusValidationRequest>
    {
        public PatientStatusValidator()
        {
            RuleFor(o => o.StatusId)
                .Equal((int) PatientStatusTypes.PendingActive)
                .When(o => o.HasOpenOrders && o.IsDMEEquipmentLeft);

            RuleFor(o => o.StatusId)
                .Equal((int) PatientStatusTypes.Pending)
                .When(o => o.HasOpenOrders && !o.IsDMEEquipmentLeft);

            RuleFor(o => o.StatusId)
                .Equal((int) PatientStatusTypes.Active)
                .When(o => !o.HasOpenOrders && o.IsDMEEquipmentLeft);

            RuleFor(o => o.StatusId)
                .Equal((int)PatientStatusTypes.Inactive)
                .When(o => !o.HasOpenOrders && !o.IsDMEEquipmentLeft & o.HasOrders);

            RuleFor(o => o.StatusId)
                .Equal((int)PatientStatusTypes.Blank)
                .When(o => !o.HasOpenOrders && !o.IsDMEEquipmentLeft & !o.HasOrders);

        }
    }
}