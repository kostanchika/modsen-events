using AutoMapper;
using EventsAPI.Models;

namespace EventsAPI.Mappers
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<CreateEventModel, Event>();
        }
    }
}
