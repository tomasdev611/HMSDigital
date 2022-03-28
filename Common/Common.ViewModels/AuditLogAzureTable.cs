using System;
using Audit.AzureTableStorage.ConfigurationApi;

namespace HMSDigital.Common.ViewModels
{
    public class AuditLogAzureTable : AuditEventTableEntity 
    {
        public string AuditAction { get; set;}

        public int UserId { get; set;}
        
        public int EntityId { get; set;}
        
        public DateTime AuditDate { get; set;}

        public string AuditData { get; set;}

        public string ClientIpAddress { get; set;}
    }
}