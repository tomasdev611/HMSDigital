using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Hms2Contracts
    {
        public Hms2Contracts()
        {
            Hms2ContractItems = new HashSet<Hms2ContractItems>();
        }

        public int Id { get; set; }
        public int Hms2ContractId { get; set; }
        public string ContractName { get; set; }
        public string ContractNumber { get; set; }
        public int? Hms2HospiceId { get; set; }
        public int? Hms2CustomerId { get; set; }
        public decimal? PerDiemRate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? HospiceId { get; set; }
        public int? HospiceLocationId { get; set; }

        public virtual Hospices Hospice { get; set; }
        public virtual HospiceLocations HospiceLocation { get; set; }
        public virtual ICollection<Hms2ContractItems> Hms2ContractItems { get; set; }
    }
}
