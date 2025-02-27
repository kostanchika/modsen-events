using AutoMapper;
using EventsAPI.DAL.Entities;
using EventsAPI.Models;
using EventsAPI.BLL.DTO;
using EventsAPI.Adapters;
using EventsAPI.ViewModels;

namespace EventsAPI.Mappers
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<CreateEventModel, CreateEventDTO>()
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? new FormFileAdapter(src.Image) : null));
            CreateMap<ChangeEventModel, ChangeEventDTO>()
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? new FormFileAdapter(src.Image) : null));
            CreateMap<EventDTO, EventViewModel>();
        }
    }
}
