using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class UserRoleBase
    {
        public int RoleId { get; set; }

        public string ResourceType { get; set; }

        public string ResourceId { get; set; }
    }
}
