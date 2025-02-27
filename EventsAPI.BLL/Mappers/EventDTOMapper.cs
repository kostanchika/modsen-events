using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.DAL.Entities;

namespace EventsAPI.BLL.Mappers
{
    public class EventDTOMapper: Profile
    {
        public EventDTOMapper()
        {
            CreateMap<CreateEventDTO, Event>()
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime.ToUniversalTime()));
            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.CurrentParticipants, opt => opt.MapFrom(src => src.Participants.Count));
        }
    }
}
