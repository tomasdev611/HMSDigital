using AutoMapper;
using HMSDigital.Fulfillment.ViewModels;

namespace HMSDigital.Fulfillment.API
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RouteRequest, OptimizeRouteRequest>()
                .ForMember(dest => dest.Agents, opt => opt.MapFrom(src => src.Drivers));

            CreateMap<Instruction, InstructionResponse>().ReverseMap();

            CreateMap<ItineraryItemRequest, ItineraryItem>().ReverseMap();
        }

    }
}