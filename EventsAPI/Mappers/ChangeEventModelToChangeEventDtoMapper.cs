using AutoMapper;
using EventsAPI.Adapters;
using EventsAPI.BLL.DTO;
using EventsAPI.Models;

namespace EventsAPI.Mappers
{
    public class ChangeEventModelToChangeEventDtoMapper : Profile
    {
        public ChangeEventModelToChangeEventDtoMapper()
        {
            CreateMap<ChangeEventModel, ChangeEventDTO>()
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? new FormFileAdapter(src.Image) : null));
        }
    }
}
