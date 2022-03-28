using FluentValidation;

namespace HMSDigital.Common.BusinessLayer.Validations
{
    public class AddressValidator : AbstractValidator<Common.ViewModels.AddressMinimal>
    {
        public AddressValidator()
        {
            RuleFor(a => a.AddressLine1).NotEmpty()
               .WithMessage(a => $"Address Line 1 must be provided");

            RuleFor(a => a.City).NotEmpty()
               .WithMessage(a => $"City must be provided");

            RuleFor(a => a.State).NotEmpty()
               .WithMessage(a => $"State must be provided");

            RuleFor(a => a.ZipCode).GreaterThanOrEqualTo(0)
                .WithMessage(a => $"A valid zip code must be provided");

            RuleFor(a => a.Plus4Code).GreaterThanOrEqualTo(0)
                .When(a => a.Plus4Code != null)
                .WithMessage(a => $"A valid plus4 code must be provided");
        }
    }
}
