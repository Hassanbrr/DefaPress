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
    public class ArticleCategoryProfile : Profile
    {

        public ArticleCategoryProfile()
        {
            CreateMap<ArticleCategory, ArticleCategoryDto>()
               
                .ForMember(dest => dest.ParentCategoryName,
                    opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            CreateMap<ArticleCategoryDto, ArticleCategory>()
             
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore()) // چون باید دستی attach شه
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore()); // برای جلوگیری از loop بی‌نهایت
        }
    }
}
