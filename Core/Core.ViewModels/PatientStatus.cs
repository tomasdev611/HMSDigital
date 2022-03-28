using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class PatientStatus
    {
        public string Status { get; set; }

        public DateTime StatusChangedDate { get; set; }

        public string Reason { get; set; }

        public bool IsDMEEquipmentLeft { get; set; }

        public bool HasOpenOrders { get; set; }
    }
}
