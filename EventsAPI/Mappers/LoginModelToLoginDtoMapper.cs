using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.Models;

namespace EventsAPI.Mappers
{
    public class LoginModelToLoginDtoMapper : Profile
    {
        public LoginModelToLoginDtoMapper()
        {
            CreateMap<LoginModel, LoginDTO>();
        }
    }
}
