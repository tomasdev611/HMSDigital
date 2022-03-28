using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.ViewModels
{
    public class StandardizedAddressResponse
    {
        public string Version { get; set; }
        
        public string TransmissionReference { get; set; }
        
        public string TransmissionResults { get; set; }
        
        public string TotalRecords { get; set; }
        
        public IEnumerable<StandardizedAddress> Records { get; set; }
    }
}
