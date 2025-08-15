using DefaPress.Repository.Context;
using DefaPress.Repository.Modules.ArticleCategory.Interfaces;
using DefaPress.Repository.Modules.Base.Implements;

namespace DefaPress.Repository.Modules.ArticleCategory.Implements;

public class ArticleCategoryRepository :Repository<Domain.ArticleCategory>,IArticleCategoryRepository
{
    public ArticleCategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
 
}