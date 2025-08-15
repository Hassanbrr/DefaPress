using AutoMapper;
using DefaPress.Application.Articles.Commands;
using DefaPress.Domain;
using DefaPress.Domain.YourProject.Domain.Entities;
using DefaPress.Repository.Modules.Base.Interfaces;
using Helps;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.Articles.Handlers
{
    public class CreateArticleHandler :IRequestHandler<CreateArticleCommand,int>
    {

        public readonly IUnitOffWork _unitOffWork;
        //private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper; // AutoMapper
        public CreateArticleHandler(IUnitOffWork unitOffWork, IMapper mapper)
        {
            _unitOffWork = unitOffWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateArticleCommand request, CancellationToken ct)
        {
            var article = new Article()
            {
                Title = request.Title,
                Slug = SlugHelper.SlugifyPersian(request.Title),
                Summary = request.Summary,
                Content = request.Content,
                ArticleCategoryId = request.ArticleCategoryId,
                IsPublished = false,
                PublishedAt = DateTime.UtcNow
            };
            // tags: create or attach existing in repo (simplified)
            foreach (var t in request.Tags.Distinct())
            {
                article.Tags.Add(new Tag { Name = t }); // or attach existing
            }

            await _unitOffWork.ArticleRepository.AddAsync(article,ct);
            await _unitOffWork.SaveChangesAsync(ct);
            return article.ArticleId;
        }
    }
}
