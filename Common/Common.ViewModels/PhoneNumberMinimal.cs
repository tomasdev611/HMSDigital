using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.ViewModels
{
    public class PhoneNumberMinimal
    {
        public long Number { get; set; }

        public int CountryCode { get; set; }

        public string NumberType { get; set; }

        public int? NumberTypeId { get; set; }

        public bool IsPrimary { get; set; }

        public bool ReceiveEtaTextmessage { get; set; }
        
        public bool ReceiveSurveyTextMessage { get; set; }
        
        public string ContactName { get; set; }
        
        public bool IsSelfPhone { get; set; }
    }
}
