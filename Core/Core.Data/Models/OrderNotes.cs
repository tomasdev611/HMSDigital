using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class OrderNotes
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public string Note { get; set; }
        public int? NetSuiteOrderNoteId { get; set; }
        public int? NetSuiteContactId { get; set; }
        public int? HospiceMemberId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual HospiceMember HospiceMember { get; set; }
        public virtual OrderHeaders OrderHeader { get; set; }
        public virtual Users UpdatedByUser { get; set; }
    }
}
