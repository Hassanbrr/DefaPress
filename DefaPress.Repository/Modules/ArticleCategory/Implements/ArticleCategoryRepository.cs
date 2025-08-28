using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.ArticleCategory.Interfaces;
using DefaPress.Infrastructure.Modules.Base.Implements;
 

namespace DefaPress.Infrastructure.Modules.ArticleCategory.Implements;

public class ArticleCategoryRepository :Repository<Domain.ArticleCategory>,IArticleCategoryRepository
{
    public ArticleCategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
 
}