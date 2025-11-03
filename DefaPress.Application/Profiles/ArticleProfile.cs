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
            // =========================
            // Entity -> List DTO
            // =========================
            CreateMap<Article, ArticleListDto>()
                // نام دسته (اگر navigation لود شده باشد)
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.ArticleCategory != null ? src.ArticleCategory.Name : null))
                // نام نویسنده (اگر navigation لود شده باشد)
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : null))
                // نام تگ‌ها (اگر collection لود شده باشد)
                .ForMember(dest => dest.TagNames,
                    opt => opt.MapFrom(src =>
                        src.Tags != null ? src.Tags.Select(t => t.Name).ToList() : new List<string>()))
                // آدرس اولین مدیا به عنوان main media (اختیاری)
                .ForMember(dest => dest.MainMediaUrl,
                    opt => opt.MapFrom(src => src.MediaFiles != null && src.MediaFiles.Any()
                        ? src.MediaFiles.OrderBy(m => m.Id).First().FileUrl
                        : null));
            // =========================
            // Entity -> Detail DTO
            // Inherit base mappings from ArticleListDto
            // =========================
            CreateMap<Article, ArticleDetailDto>()
                .IncludeBase<Article, ArticleListDto>()
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.MediaUrls,
                    opt => opt.MapFrom(src =>
                        src.MediaFiles != null ? src.MediaFiles.Select(m => m.FileUrl).ToList() : new List<string>()))
                .ForMember(dest => dest.Source,
                    opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.UpdatedAt,
                    opt => opt.MapFrom(src => src.UpdatedAt));
       
            
            CreateMap<ArticleCreateDto, Article>()
                .ForMember(dest => dest.ArticleId, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ForMember(dest => dest.MediaFiles, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<ArticleUpdateDto, Article>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ForMember(dest => dest.MediaFiles, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // CreatedAt نباید از DTO بازنویسی بشه
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); // می‌تونیم UpdatedAt رو ست کنیم


        }
    }
}
