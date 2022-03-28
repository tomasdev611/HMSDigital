using Microsoft.WindowsAzure.Storage.Table;

namespace HMSDigital.Core.BusinessLayer.DTOs
{
    public class APILogs : TableEntity
    {
        public string RenderedMessage { get; set; }

        public string Level { get; set; }

        public string Exception { get; set; }

        public string Data { get; set; }
    }
}
