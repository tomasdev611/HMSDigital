using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class PermissionNouns
    {
        public PermissionNouns()
        {
            RolePermissions = new HashSet<RolePermissions>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
