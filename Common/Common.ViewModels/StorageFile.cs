using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HMSDigital.Common.ViewModels
{
    public class StorageFile
    {
        public FileMetadataRequest FileMetadata { get; set; }

        public Stream FileContent { get; set; }

        public string StorageFilePath { get; set; }
    }
}
