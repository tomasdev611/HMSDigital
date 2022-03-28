using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class CustomerTypes
    {
        public CustomerTypes()
        {
            HospiceLocations = new HashSet<HospiceLocations>();
            Hospices = new HashSet<Hospices>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<HospiceLocations> HospiceLocations { get; set; }
        public virtual ICollection<Hospices> Hospices { get; set; }
    }
}
