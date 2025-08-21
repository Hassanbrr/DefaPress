using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Domain;

namespace DefaPress.Application.Profiles
{
    public class ArticleProfile : Profile   

    {
        public ArticleProfile()
        {
            // Entity -> DTO
            CreateMap<Article, ArticleDto>()
                // Category name from navigation
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.ArticleCategory.Name))
                // Author name from navigation 
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author.FullName))
                // Tags collection to list of tag names
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()))
                // Count comments safely
                .ForMember(dest => dest.CommentsCount,
                    opt => opt.MapFrom(src => src.Comments.Count));

            // DTO -> Entity
            CreateMap<ArticleDto, Article>()
               
                .ForMember(dest => dest.ArticleCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore());
        }
    }
}
