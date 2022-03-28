using AutoMapper;
using HMSDigital.Notification.Data.Models;
using HMSDigital.Notification.ViewModels;

namespace HMSDigital.Notification.API
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DeviceRegister, PushNotificationMetadata>()
                .ReverseMap();
        }
    }
}