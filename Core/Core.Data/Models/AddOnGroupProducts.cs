using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class AddOnGroupProducts
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int? ItemId { get; set; }

        public virtual AddOnGroups Group { get; set; }
        public virtual Items Item { get; set; }
    }
}
