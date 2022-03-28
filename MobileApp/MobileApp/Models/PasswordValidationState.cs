using System;
namespace MobileApp.Models
{
    public class PasswordValidationState
    {
        public bool IsContainsLowerCase { get; set; }

        public bool IsContainsUpperCase { get; set; }

        public bool IsContainsNumber { get; set; }

        public bool IsContainsSpecialChar { get; set; }

        public bool IsContainsMinimumLength { get; set; }

        public bool IsPasswordValid { get; set; }
    }
}
