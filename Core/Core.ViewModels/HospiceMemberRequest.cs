using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class HospiceMemberRequest : UserMinimal
    {
        public string Designation { get; set; }

        public IEnumerable<HospiceMemberRoleRequest> UserRoles { get; set; }

        public bool CanAccessWebStore { get; set; }
    }
}
