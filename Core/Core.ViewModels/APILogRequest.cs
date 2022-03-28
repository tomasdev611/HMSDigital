using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace HMSDigital.Core.ViewModels
{
    public class APILogRequest
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        private string _apiLogType;

        public string APILogType
        {
            get 
            {
                if (!string.IsNullOrEmpty(_apiLogType))
                {
                    return _apiLogType;
                }
                else
                {
                    return "core";
                }
            }
            set 
            { _apiLogType = value; }
        }


        public TableContinuationToken ContinuationToken { get; set; }

        public int PageSize { get; set; }

        public int? UserId { get; set; }

        public string ActionType { get; set; }

        public int? EntityId { get; set; }
    }
}
