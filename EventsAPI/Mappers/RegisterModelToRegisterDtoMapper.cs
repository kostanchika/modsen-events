using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.Models;

namespace EventsAPI.Mappers
{
    public class RegisterModelToRegisterDtoMapper : Profile
    {
        public RegisterModelToRegisterDtoMapper()
        {
            CreateMap<RegisterModel, RegisterDTO>();
        }
    }
}
