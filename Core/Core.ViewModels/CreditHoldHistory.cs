using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class CreditHoldHistory
    {
        public int Id { get; set; }

        public int HospiceId { get; set; }

        public string CreditHoldNote { get; set; }

        public DateTime CreditHoldDateTime { get; set; }

        public bool IsCreditOnHold { get; set; }

        public string CreditHoldByUserName { get; set; }
    }
}
