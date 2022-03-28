using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Notification.BusinessLayer;
using HMSDigital.Notification.BusinessLayer.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Notification.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Test.MockProvider
{
    public class MockServices
    {
        private readonly IServiceProvider _serviceProvider;

        public MockServices()
        {
            var mockRepository = new MockRepository();

            var services = new ServiceCollection()
               .AddAutoMapper(typeof(Startup));

            services.AddLogging();
            services.AddScoped(s => GetNotificationService());
            services.AddScoped(s => GetQueueClient());
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped(s => new MockRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetNotificationMetaDataRepository());
            _serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        public INotificationService GetNotificationService()
        {
            var notificationService = new Mock<INotificationService>();
            return notificationService.Object;
        }

        public IQueueClient GetQueueClient()
        {
            var queueClient = new Mock<IQueueClient>();
            queueClient.Setup(x => x.SendAsync(It.IsAny<IList<Message>>())).Returns(Task.Delay(1));
            return queueClient.Object;
        }

    }
}
