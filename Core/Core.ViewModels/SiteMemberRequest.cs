using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class SiteMemberRequest : UserMinimal
    {
        public string Designation { get; set; }

        public IEnumerable<int> RoleIds { get; set; }
    }
}
