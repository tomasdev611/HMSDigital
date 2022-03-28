using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class HospiceMemberCsvDTO : HospiceMemberCsvRequest
    {
        public IEnumerable<int> HospiceLocationIds { get; set; }

        public IEnumerable<HospiceMemberRoleRequest> UserRoles { get; set; }
    }
}
