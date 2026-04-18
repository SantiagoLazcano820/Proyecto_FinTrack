using AutoMapper;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;

namespace FinTrack.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
