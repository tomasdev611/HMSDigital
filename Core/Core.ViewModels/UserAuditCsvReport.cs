using System;

namespace HMSDigital.Core.ViewModels
{
    public class UserAuditCsvReport
    {
        public int AuditId { get; set; }
        public string AuditAction { get; set; }
        public string ColumnName { get; set; }
        public object OriginalValue { get; set; }
        public object NewValue { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserCognitoId { get; set; }
        public int TargetUserId { get; set; }
        public string TargetUserName { get; set; }
        public string TargetUserCognitoId { get; set; }
        public DateTime AuditDate { get; set; }
        public string ClientIPAddress { get; set; }

    }
}
