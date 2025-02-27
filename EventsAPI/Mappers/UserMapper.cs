using AutoMapper;
using EventsAPI.Models;
using EventsAPI.BLL.DTO;

namespace EventsAPI.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterModel, RegisterDTO>();
            CreateMap<LoginModel, LoginDTO>();
        }

    }
}
