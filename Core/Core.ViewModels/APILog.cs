using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class APILog
    {
        public string RenderedMessage { get; set; }

        public string Level { get; set; }

        public string Exception { get; set; }

        public string Data { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
