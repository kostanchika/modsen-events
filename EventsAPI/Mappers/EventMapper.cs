using AutoMapper;
using EventsAPI.DAL.Entities;
using EventsAPI.Models;

namespace EventsAPI.Mappers
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<CreateEventModel, Event>()
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime.ToUniversalTime()));
            CreateMap<Event, GetEventsResponse>()
                .ForMember(dest => dest.CurrentParticipants, opt => opt.MapFrom(src => src.Participants.Count));
        }
    }
}
