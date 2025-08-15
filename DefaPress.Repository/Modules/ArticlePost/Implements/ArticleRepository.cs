using DefaPress.Repository.Context;
using DefaPress.Repository.Modules.ArticlePost.Interfaces;
using DefaPress.Repository.Modules.Base.Implements;

namespace DefaPress.Repository.Modules.ArticlePost.Implements;

public class ArticleRepository : Repository<Domain.Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext context) : base(context)
    {
    }

  
}
