using AutoMapper;
using EventsAPI.Models;

namespace EventsAPI.Mappers
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<CreateEventModel, Event>();
            CreateMap<Event, GetEventsResponse>()
                .ForMember(dest => dest.CurrentParticipants, opt => opt.MapFrom(src => src.Participants.Count));
        }
    }
}
