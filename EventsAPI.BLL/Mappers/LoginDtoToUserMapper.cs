using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.DAL.Entities;

namespace EventsAPI.BLL.Mappers
{
    public class LoginDtoTouserMapper : Profile
    {
        public LoginDtoTouserMapper()
        {
            CreateMap<LoginDTO, User>();
        }
    }
}
