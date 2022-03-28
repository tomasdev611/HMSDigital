using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class PatientOrderRequest
    {
        public Guid PatientUUID { get; set; }

        public string OrderNumber { get; set; }
    }
}
