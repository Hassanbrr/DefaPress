using AutoMapper;
using DefaPress.Application.ArticleCategories.Dtos;
using DefaPress.Application.Articles.Commands;
using DefaPress.Application.Articles.Dtos;
using DefaPress.Application.Comments.Dtos;
using DefaPress.Domain;
using DefaPress.Domain.YourProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using DefaPress.Application.Tags.Dtos;

namespace DefaPress.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ---------------------
            // Entity -> DTO mappings
            // ---------------------

            CreateMap<Article, ArticleDto>()
                // simple scalar mappings are automatic
                .ForMember(d => d.CategoryName,
                    opt => opt.MapFrom(s => s.ArticleCategory != null ? s.ArticleCategory.Name : null))
                .ForMember(d => d.AuthorName,
                    opt => opt.MapFrom(s => s.Author != null ? (s.Author.FullName ?? s.Author.UserName) : null))
                // map Tags collection to simple list of names (ProjectTo will translate this)
                .ForMember(d => d.Tags,
                    opt => opt.MapFrom(s => s.Tags.Select(t => t.Name)));

            CreateMap<ArticleCategory, ArticleCategoryDto>()
                .ForMember(d => d.ParentCategoryName,
                    opt => opt.MapFrom(s => s.ParentCategory != null ? s.ParentCategory.Name : null))
                // recursive mapping for subcategories (AutoMapper will reuse this mapping)
                .ForMember(d => d.SubCategories,
                    opt => opt.MapFrom(s => s.SubCategories));

            CreateMap<Tag, TagDto>();

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.UserName,
                    opt => opt.MapFrom(s => s.User != null ? (s.User.FullName ?? s.User.UserName) : null))
                .ForMember(d => d.ArticleTitle,
                    opt => opt.MapFrom(s => s.Article != null ? s.Article.Title : null));


            // ---------------------
            // Command -> Entity mappings (Commands are DTO-like inputs)
            // ---------------------
            // Map CreateArticleCommand -> Article (basic fields). 
            // Ignore navigation-related properties that require DB lookups (Tags, Author navigation, Slug generation).
            if (Type.GetType("Application.Articles.Commands.CreateArticleCommand") != null)
            {
                CreateMap<CreateArticleCommand, Article>()
                    .ForMember(d => d.ArticleId, o => o.Ignore())
                    .ForMember(d => d.Slug, o => o.Ignore())
                    .ForMember(d => d.Tags, o => o.Ignore())
                    .ForMember(d => d.Author, o => o.Ignore())
                    .ForMember(d => d.IsPublished, o => o.Ignore())
                    .ForMember(d => d.PublishedAt, o => o.Ignore());
            }

            // ---------------------
            // Global mapping options
            // ---------------------
            // prevent infinite recursion in self-referencing graphs (e.g. Category -> SubCategories -> Parent)
            ((IProfileExpressionInternal)this).ForAllMaps((typeMap, map) => map.MaxDepth(3));
        }
    }
}
