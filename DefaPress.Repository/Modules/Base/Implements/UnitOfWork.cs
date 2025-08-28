using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Article.Interfaces;
using DefaPress.Infrastructure.Modules.ArticleCategory.Interfaces;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
 

namespace DefaPress.Infrastructure.Modules.Base.Implements
{
    public class UnitOfWork : IUnitOffWork
    {
        private readonly ApplicationDbContext _context;
    



        public IArticleRepository ArticleRepository { get; private set; }
        public IArticleCategoryRepository ArticleCategoryRepository { get; private set; }


        public UnitOfWork(ApplicationDbContext context, IArticleRepository articleRepository, IArticleCategoryRepository articleCategoryRepository)
        {
            _context = context;
            ArticleRepository = articleRepository;
            ArticleCategoryRepository = articleCategoryRepository;
        }

    
        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
