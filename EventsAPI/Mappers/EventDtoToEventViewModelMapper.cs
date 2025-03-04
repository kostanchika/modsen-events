using AutoMapper;
using EventsAPI.DAL.Entities;
using EventsAPI.Models;
using EventsAPI.BLL.DTO;
using EventsAPI.Adapters;
using EventsAPI.ViewModels;

namespace EventsAPI.Mappers
{
    public class EventDtoToEventViewModelMapper : Profile
    {
        public EventDtoToEventViewModelMapper()
        {
            CreateMap<EventDTO, EventViewModel>();
        }
    }
}
