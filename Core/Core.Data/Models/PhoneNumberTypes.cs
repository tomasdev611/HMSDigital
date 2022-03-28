using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class PhoneNumberTypes
    {
        public PhoneNumberTypes()
        {
            PhoneNumbers = new HashSet<PhoneNumbers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PhoneNumbers> PhoneNumbers { get; set; }
    }
}
