using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientDetail : PatientMinimal
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public string LastOrderNumber { get; set; }

        public DateTime? LastOrderDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string Status { get; set; }

        public int? StatusId { get; set; }

        public string StatusColor { get;set; }

        public string StatusInfo { get;set; }

        public DateTime? StatusChangedDate { get; set; }

        public string StatusReason { get; set; }

        public int CreatedByUserId { get; set; }

        public IEnumerable<PatientAddress> PatientAddress { get; set; }
    }
}
