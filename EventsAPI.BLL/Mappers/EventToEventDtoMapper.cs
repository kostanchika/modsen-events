using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.DAL.Entities;

namespace EventsAPI.BLL.Mappers
{
    public class EventToEventDtoMapper: Profile
    {
        public EventToEventDtoMapper()
        {
            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.CurrentParticipants, opt => opt.MapFrom(src => src.Participants.Count));
        }
    }
}
