using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Notification.BusinessLayer.Interfaces;
using HMSDigital.Notification.Data.Models;
using HMSDigital.Notification.Data.Repositories.Interfaces;
using HMSDigital.Notification.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Notification.BusinessLayer
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly NotificationHubConfig _notificationHubConfig;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;

        private readonly IPushNotificationMetaDataRepository _pushNotificationMetaDataRepository;

        public PushNotificationService(IOptions<NotificationHubConfig> notificationHubOptions,
                                        IHttpContextAccessor httpContextAccessor,
                                        IMapper mapper,
                                        IPushNotificationMetaDataRepository pushNotificationMetaDataRepository)
        {
            _notificationHubConfig = notificationHubOptions.Value;
            _httpContext = httpContextAccessor.HttpContext;
            _mapper = mapper;
            _pushNotificationMetaDataRepository = pushNotificationMetaDataRepository;
        }

        public async Task<DeviceRegister> GetDeviceRegistrationByDeviceId(string deviceId)
        {
            var deviceMetaData = await _pushNotificationMetaDataRepository.GetAsync(p => p.DeviceId == deviceId); ;
            return _mapper.Map<DeviceRegister>(deviceMetaData);
        }

        public async Task DeviceRegistration(DeviceRegister deviceRegisterRequest)
        {
            var deviceTags = new List<string>();
            var deviceMetaData = await _pushNotificationMetaDataRepository.GetAsync(p => p.DeviceId == deviceRegisterRequest.DeviceId);
            if (deviceMetaData == null && deviceRegisterRequest.InstallationId == Guid.Empty)
            {
                deviceRegisterRequest.InstallationId = Guid.NewGuid();
            }
            else
            {
                deviceRegisterRequest.InstallationId = deviceMetaData.InstallationId;
            }
            
            var notificationClient = GetNotificationHubClient();

            var userId = GetLoggedInUserId();
            if (userId != null)
            {
                deviceTags.Add($"userId:{userId}");
            }
            if (deviceRegisterRequest.CurrentSiteId != null)
            {
                deviceTags.Add($"currentSiteId:{deviceRegisterRequest.CurrentSiteId}");
            }
            var installation = new Installation()
            {
                InstallationId = deviceRegisterRequest.InstallationId.ToString(),
                PushChannel = deviceRegisterRequest.DeviceId,
                Tags = deviceTags.ToArray()
            };

            switch (deviceRegisterRequest.Platform.ToLower())
            {
                case "ios":
                    installation.Platform = NotificationPlatform.Apns;
                    break;
                case "android":
                    installation.Platform = NotificationPlatform.Fcm;
                    break;
            }

            await notificationClient.CreateOrUpdateInstallationAsync(installation);
            
            if (deviceMetaData != null)
            {
                deviceMetaData = _mapper.Map(deviceRegisterRequest, deviceMetaData);
                deviceMetaData.UserId = userId;
                await _pushNotificationMetaDataRepository.UpdateAsync(deviceMetaData);
            }
            else
            {
                var notificationMetaData = _mapper.Map<PushNotificationMetadata>(deviceRegisterRequest);
                notificationMetaData.UserId = userId;
                await _pushNotificationMetaDataRepository.AddAsync(notificationMetaData);
            }
        }

        public async Task DeleteDeviceRegistration(string deviceId)
        {
            var deviceMetaData = await _pushNotificationMetaDataRepository.GetAsync(p => p.DeviceId == deviceId);
            if(deviceMetaData == null)
            {
                return;
            }
            var notificationClient = GetNotificationHubClient();
            await notificationClient.DeleteInstallationAsync(deviceMetaData.InstallationId.ToString());
            await _pushNotificationMetaDataRepository.DeleteAsync(deviceMetaData);
        }

        private NotificationHubClient GetNotificationHubClient()
        {
            return new NotificationHubClient(_notificationHubConfig.ConnectionString, _notificationHubConfig.HubName);
        }
        private int? GetLoggedInUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return null;
            }
            return int.Parse(userIdClaim.Value);
        }
    }
}
