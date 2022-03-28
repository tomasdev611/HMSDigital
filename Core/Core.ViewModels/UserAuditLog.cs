
namespace HMSDigital.Core.ViewModels
{
    public class UserAuditLog : AuditLog
    {
        public int TargetUserId { get; set; }

        public User TargetUser { get; set; }

    }
}
