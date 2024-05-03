using AutoMapper;
using loginService.Dto;
using loginService.Models;

namespace loginService.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
