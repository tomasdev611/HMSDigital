using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public bool IsStatic { get; set; }

        public string RoleType { get; set; }

        public virtual IEnumerable<string> Permissions { get; set; }

        public int PermissionsLength { get; set; }
    }
}
