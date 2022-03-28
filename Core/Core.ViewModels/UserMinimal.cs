using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class UserMinimal
    {
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value?.ToLower(); }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CountryCode { get; set; }

        public long PhoneNumber { get; set; }
    }
}
