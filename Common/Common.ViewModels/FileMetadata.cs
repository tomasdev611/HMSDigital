using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.ViewModels
{
    public class FileMetadata : FileMetadataRequest
    {
        public int StorageTypeId { get; set; }

        public string StorageRoot { get; set; }

        public string StorageFilePath { get; set; }
    }
}
