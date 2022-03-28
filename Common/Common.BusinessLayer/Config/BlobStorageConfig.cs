using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.BusinessLayer.Config
{
    public class BlobStorageConfig
    {
        public string ConnectionString { get; set; }

        public StorageContainersConfig Containers { get; set; }

        public double DownloadUrlExpiresInMinutes { get; set; }

        public double UploadUrlExpiresInMinutes { get; set; }
    }
}
