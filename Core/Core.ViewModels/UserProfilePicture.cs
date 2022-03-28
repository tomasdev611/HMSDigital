using HMSDigital.Common.ViewModels;
using System;

namespace HMSDigital.Core.ViewModels
{
    public class UserProfilePicture : FileMetadata
    {
        public int Id { get; set; }

        public bool IsUploaded { get; set; }

        public Uri UploadUrl { get; set; }

        public Uri DownloadUrl { get; set; }
    }
}
