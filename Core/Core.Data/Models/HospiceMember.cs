using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class HospiceMember
    {
        public HospiceMember()
        {
            HospiceLocationMembers = new HashSet<HospiceLocationMembers>();
            OrderHeaders = new HashSet<OrderHeaders>();
            OrderNotes = new HashSet<OrderNotes>();
        }

        public int Id { get; set; }
        public int HospiceId { get; set; }
        public int UserId { get; set; }
        public string Designation { get; set; }
        public int? NetSuiteContactId { get; set; }
        public bool? CanAccessWebStore { get; set; }
        public bool? CanApproveOrder { get; set; }
        public bool? AdditionalField1 { get; set; }
        public int? AdditionalField2 { get; set; }
        public DateTime? AdditionalField3 { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Hospices Hospice { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<HospiceLocationMembers> HospiceLocationMembers { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeaders { get; set; }
        public virtual ICollection<OrderNotes> OrderNotes { get; set; }
    }
}
