using AutoMapper;
using EventsAPI.BLL.DTO;
using EventsAPI.DAL.Entities;

namespace EventsAPI.BLL.Mappers
{
    public class UserToUserDtoMapper: Profile
    {
        public UserToUserDtoMapper()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
