using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsAPI.BLL.Mappers
{
    public class CreateEventDtoToEventMapper : Profile
    {
        public CreateEventDtoToEventMapper()
        {
            CreateMap<CreateEventDTO, Event>()
                .ForMember(dest => dest.EventDateTime, opt => opt.MapFrom(src => src.EventDateTime.ToUniversalTime()));
        }
    }
}
