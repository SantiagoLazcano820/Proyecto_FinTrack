using AutoMapper;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;

namespace FinTrack.Infrastructure.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => (ulong)1));
        }
    }
}
