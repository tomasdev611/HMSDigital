using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionNounId { get; set; }
        public int PermissionVerbId { get; set; }

        public virtual PermissionNouns PermissionNoun { get; set; }
        public virtual PermissionVerbs PermissionVerb { get; set; }
        public virtual Roles Role { get; set; }
    }
}
