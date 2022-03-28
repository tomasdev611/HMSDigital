using HMSDigitalModel = HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HMSDigital.Notification.Data.Repositories.Interfaces;
using HMSDigital.Notification.Data.Models;

namespace Notification.Test.MockProvider
{
    public class MockRepository
    {
        public MockRepository()
        {
        }

        public IPushNotificationMetaDataRepository GetNotificationMetaDataRepository()
        {
            var pushNotificationMetaDataRepository = new Mock<IPushNotificationMetaDataRepository>();
            return pushNotificationMetaDataRepository.Object;
        }
    }
}
