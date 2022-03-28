using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class CreditHoldHistory
    {
        public int Id { get; set; }
        public int HospiceId { get; set; }
        public int CreditHoldByUserId { get; set; }
        public string CreditHoldNote { get; set; }
        public DateTime CreditHoldDateTime { get; set; }
        public bool IsCreditOnHold { get; set; }

        public virtual Users CreditHoldByUser { get; set; }
        public virtual Hospices Hospice { get; set; }
    }
}
