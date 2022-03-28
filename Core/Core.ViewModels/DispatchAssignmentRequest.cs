using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchAssignmentRequest
    {
        public int VehicleId { get; set; }

        public IEnumerable<DispatchInstructionRequest> DispatchDetails { get; set; }
    }
}
