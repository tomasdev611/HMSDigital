using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class HospiceMemberRoleRequest
    {
        public int? RoleId { get; set; }

        public string ResourceType { get; set; } //Hospice or HospiceLocation

        public int ResourceId { get; set; } // hospiceId or hospiceLocationId
    }
}
