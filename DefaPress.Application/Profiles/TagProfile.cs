using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;

namespace DefaPress.Application.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            // Entity -> DTO
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.ArticleCount,
                    opt => opt.MapFrom(src => src.Articles != null ? src.Articles.Count : 0));

            // Create DTO -> Entity
            CreateMap<TagCreateDto, Tag>()
                .ForMember(dest => dest.TagId, opt => opt.Ignore())
                .ForMember(dest => dest.Articles, opt => opt.Ignore());

            // Update DTO -> Entity
            CreateMap<TagUpdateDto, Tag>()
                .ForMember(dest => dest.Articles, opt => opt.Ignore());
        }
    }
}