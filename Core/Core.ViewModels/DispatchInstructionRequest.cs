using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchInstructionRequest
    {
        public int? OrderHeaderId { get; set; }
        
        public int? TransferRequestId { get; set; }

        public int? SequenceNumber { get; set; }

        public DateTime DispatchStartDateTime { get; set; }

        public DateTime DispatchEndDateTime { get; set; }
    }
}
