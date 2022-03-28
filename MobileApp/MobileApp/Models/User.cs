using System.Collections.Generic;

namespace MobileApp.Models
{
    public class User
    {
        public long id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CountryCode { get; set; }

        public long PhoneNumber { get; set; }
    }
}
