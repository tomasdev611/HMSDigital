using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class UserRoles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ResourceId { get; set; }
        public string ResourceType { get; set; }
        public int? RoleId { get; set; }

        public virtual Roles Role { get; set; }
        public virtual Users User { get; set; }
    }
}
