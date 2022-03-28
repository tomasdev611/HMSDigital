using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchInstruction : DispatchInstructionRequest
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public OrderHeader OrderHeader { get; set; }

        public ItemTransferRequest TransferRequest { get; set; }
    }
}
