using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.ViewModels
{
    public class FileMetadataRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ContentType { get; set; }

        public long SizeInBytes { get; set; }

        public bool IsPublic { get; set; }
    }
}
