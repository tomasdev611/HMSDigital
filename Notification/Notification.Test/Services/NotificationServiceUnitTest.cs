using HMSDigital.Notification.BusinessLayer.Interfaces;
using Microsoft.Azure.ServiceBus;
using Notification.Test.MockProvider;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Notification.Test.Services
{



    public class NotificationServiceUnitTest
    {
        private readonly INotificationService _notificationService;
        private readonly MockViesModels _mockViesModels;

        public NotificationServiceUnitTest()
        {
            var mockService = new MockServices();
            _notificationService = mockService.GetService<INotificationService>();
            _mockViesModels = new MockViesModels();
           
        }

        [Fact]
        public async Task CanSendNotification()
        {
            try
            {
                await _notificationService.PostNotification(_mockViesModels.GetNotificationPostRequest());
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }
}
