using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class MergePatientRequest : MergePatientBaseRequest
    {
        public bool IsDMEEquipmentLeft { get; set; }

        public bool HasOpenOrders { get; set; }

        public int? MergedByUserId { get; set; }
    }
}
