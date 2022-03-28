using HMSDigital.Notification.Data.Models;
using HMSDigital.Notification.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Notification.Data.Repositories
{
    public class PushNotificationMetaDataRepository : RepositoryBase<PushNotificationMetadata>, IPushNotificationMetaDataRepository
    {
        public PushNotificationMetaDataRepository(HMSNotificationAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
    }
}
