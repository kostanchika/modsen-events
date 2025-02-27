using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Services;
using EventsAPI.DAL.Entities;

namespace EventsAPI.BLL.Mappers
{
    public class UserDTOMapper: Profile
    {
        public UserDTOMapper()
        {
            CreateMap<LoginDTO, User>();
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => new PasswordService().HashPassword(src.Password)));
            CreateMap<User, UserDTO>();
        }
    }
}
