using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class UserBase : UserMinimal
    {
        public string Name { get; set; }

        public bool? IsEmailVerified { get; set; }

        public bool? IsPhoneNumberVerified { get; set; }

        public string UserStatus { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
