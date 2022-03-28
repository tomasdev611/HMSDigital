using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class AddOnGroups
    {
        public AddOnGroups()
        {
            AddOnGroupProducts = new HashSet<AddOnGroupProducts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool AllowMultipleSelection { get; set; }
        public int ItemId { get; set; }

        public virtual Items Item { get; set; }
        public virtual ICollection<AddOnGroupProducts> AddOnGroupProducts { get; set; }
    }
}
