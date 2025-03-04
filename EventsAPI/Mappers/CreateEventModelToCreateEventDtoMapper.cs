using AutoMapper;
using EventsAPI.Adapters;
using EventsAPI.BLL.DTO;
using EventsAPI.Models;

namespace EventsAPI.Mappers
{
    public class CreateEventModelToCreateEventDtoMapper : Profile
    {
        public CreateEventModelToCreateEventDtoMapper()
        {
            CreateMap<CreateEventModel, CreateEventDTO>()
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? new FormFileAdapter(src.Image) : null));
        }
    }
}
