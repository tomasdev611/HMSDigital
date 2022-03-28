using System;

namespace HMSDigital.Core.ViewModels
{
    public class OrderNote
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public int NetSuiteContactId { get; set; }

        public int NetSuiteOrderNoteId { get; set; }

        public int? HospiceMemberId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }
    }
}
