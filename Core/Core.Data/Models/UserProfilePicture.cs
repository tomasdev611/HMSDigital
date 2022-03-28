using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class UserProfilePicture
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FileMetadataId { get; set; }
        public bool IsUploaded { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public string DownloadUrl { get; set; }
        public DateTime? CacheExpiryDateTime { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual FilesMetadata FileMetadata { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual Users User { get; set; }
    }
}
