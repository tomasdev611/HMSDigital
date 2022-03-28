using FluentValidation;
using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class UserProfilePictureValidator : AbstractValidator<UserPictureFileRequest>
    {
        public UserProfilePictureValidator()
        {
            RuleFor(pi => pi.ContentType).NotEmpty()
               .WithMessage(p => $"Invalid Content Type")
               .Must(CheckContentTypes)
               .WithMessage(pi=> $"Content Type should be jpg, jpeg or png");
        }

       public bool CheckContentTypes(string contentType)
        { 
            var validImageTypes = new List<string>() { "jpg", "png", "jpeg"};
            if (!validImageTypes.Contains(contentType))
            {
                return false;
            }
            return true;
        }
    }
}
