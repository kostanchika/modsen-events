using AutoMapper;
using EventsAPI.DAL.Entities;
using EventsAPI.Models;
using EventsAPI.Services;

namespace EventsAPI.Mappers
{
    public class UserMapper : Profile
    {
        private static PasswordService _passwordService = new PasswordService();
        public UserMapper()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => HashPassword(src.Password)));
        }

        private static string HashPassword(string password)
        {
            return _passwordService.HashPassword(password);
        }
    }
}
