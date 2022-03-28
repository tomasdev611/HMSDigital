using System.Collections.Generic;

namespace MobileApp.Models
{
    public class Role
    {
        public int Id { get; set; }

        public virtual IEnumerable<string> Permissions { get; set; }
    }
}
