using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DefaPress.Domain;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Profiles
{
    public class ArticleCategoryProfile : Profile
    {
        public ArticleCategoryProfile()
        {
            // Entity -> DTO
            // تنظیم MaxDepth/PreserveReferences برای جلوگیری از loop هنگام map کردن ساختار درختی
            CreateMap<ArticleCategory, ArticleCategoryDto>()
                .ForMember(dest => dest.ParentCategoryName,
                    opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
                .ForMember(dest => dest.SubCategories,
                    opt => opt.MapFrom(src => src.SubCategories != null
                        ? src.SubCategories.OrderBy(sc => sc.DisplayOrder).ToList()
                        : new List<ArticleCategory>())) // AutoMapper خودش هر ArticleCategory را به ArticleCategoryDto map خواهد کرد
                .PreserveReferences()    // جلوگیری از ارجاع‌های دوتایی که ممکنه loop بسازند
                .MaxDepth(5);            // حداکثر عمق نگاشت درخت (بسته به نیازت می‌تونی کم/زیاد کنی)

            // DTO -> Entity (برای Create/Update)
            // توجه: ParentCategory و SubCategories را Ignore می‌کنیم تا کنترل attach/relationship را در سرویس داشته باشی.
            CreateMap<ArticleCategoryDto, ArticleCategory>()
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));

            // اگر DTO جداگانه‌ای برای Create داری (ArticleCategoryCreateDto) بهتر است آنرا هم داشته باشی:
            CreateMap<ArticleCategoryCreateDto, ArticleCategory>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore());
        }
    }
}
