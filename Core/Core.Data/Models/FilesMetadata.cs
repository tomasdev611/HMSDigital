using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class FilesMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public int SizeInBytes { get; set; }
        public int StorageTypeId { get; set; }
        public string StorageRoot { get; set; }
        public string StorageFilePath { get; set; }
        public bool IsPublic { get; set; }

        public virtual StorageTypes StorageType { get; set; }
        public virtual ItemImageFiles ItemImageFiles { get; set; }
        public virtual UserProfilePicture UserProfilePicture { get; set; }
    }
}
