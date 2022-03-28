using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class StorageTypes
    {
        public StorageTypes()
        {
            FilesMetadata = new HashSet<FilesMetadata>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FilesMetadata> FilesMetadata { get; set; }
    }
}
