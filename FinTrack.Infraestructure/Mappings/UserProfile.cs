using AutoMapper;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;

namespace FinTrack.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId == 0 ? 2 : src.RoleId));
        }
    }
}
