using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Services;
using EventsAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsAPI.BLL.Mappers
{
    public class RegisterDtoToUserMapper : Profile
    {
        public RegisterDtoToUserMapper()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => new PasswordService().HashPassword(src.Password)));
        }
    }
}
