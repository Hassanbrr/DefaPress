using AutoMapper;
using DefaPress.Application.DTOs;
using DefaPress.Application.Interfaces;
using DefaPress.Domain;
using DefaPress.Domain.YourProject.Domain.Entities;
using DefaPress.Repository.Context;
using DefaPress.Repository.Modules.Base.Implements;
using DefaPress.Repository.Modules.Base.Interfaces;
using Helps;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.Services
{
    public class ArticleService : IArticleService
    {

        public readonly IUnitOffWork _unitOfWork;
        public readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleService(IUnitOffWork unitOffWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOffWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<ArticleDto>> GetAllArticlesAsync()
        {
            var articles = await _unitOfWork.ArticleRepository.GetAllAsync("ArticleCategory,Author,Comments,Tags");
            return _mapper.Map<IEnumerable<ArticleDto>>(articles);

        }

        public async Task<ArticleDto> GetArticleByIdAsync(int id)
        {
            var article = await _unitOfWork.ArticleRepository.GetByIdAsync(id);
            return _mapper.Map<ArticleDto>(article);
        }




        public async Task AddArticleAsync(ArticleDto articleDto)
        {

            var article = _mapper.Map<Article>(articleDto);


            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            article.AuthorId = userId;


            article.ArticleCategoryId = articleDto.ArticleCategoryId;
            var baseSlug = string.IsNullOrWhiteSpace(articleDto.Slug)
                ? SlugHelper.SlugifyPersian(articleDto.Title)
                : SlugHelper.SlugifyPersian(articleDto.Slug);

            article.Slug = baseSlug;

            article.IsPublished = articleDto.IsPublished;
            article.PublishedAt = articleDto.IsPublished
                ? (articleDto.PublishedAt?.ToUniversalTime() ?? DateTime.UtcNow)
                : null;


            article.Tags = articleDto.Tags?
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => new Tag { Name = t.Trim() })
                .DistinctBy(t => t.Name, StringComparer.OrdinalIgnoreCase) 
                .ToList() ?? new List<Tag>();

            await _unitOfWork.ArticleRepository.AddAsync(article);
            await _unitOfWork.SaveChangesAsync();
        }


        public Task UpdateArticleAsync(ArticleDto articleDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteArticleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task PublishArticleAsync(int articleId)
        {
            var article = await _unitOfWork.ArticleRepository.GetByIdAsync(articleId);
            if (article == null) throw new KeyNotFoundException("Article not found.");

            if (!article.IsPublished)
            {
                article.IsPublished = true;
                article.PublishedAt = DateTime.UtcNow;
                _unitOfWork.ArticleRepository.Update(article);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task UnpublishArticleAsync(int articleId)
        {
            var article = await _unitOfWork.ArticleRepository.GetByIdAsync(articleId);
            if (article == null) throw new KeyNotFoundException("Article not found.");

            article.IsPublished = false;
            _unitOfWork.ArticleRepository.Update(article);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
