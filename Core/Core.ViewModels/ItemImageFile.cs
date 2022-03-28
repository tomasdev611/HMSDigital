using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class ItemImageFile : FileMetadata
    {
        public int Id { get; set; }

        public bool IsUploaded { get; set; }

        public Uri UploadUrl { get; set; }

        public Uri DownloadUrl { get; set; }
    }
}
