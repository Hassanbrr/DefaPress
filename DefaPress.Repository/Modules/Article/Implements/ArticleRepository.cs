using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Article.Interfaces;
using DefaPress.Infrastructure.Modules.Base.Implements;
using Microsoft.EntityFrameworkCore;


namespace DefaPress.Infrastructure.Modules.Article.Implements;

public class ArticleRepository : Repository<Domain.Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext context) : base(context)
    {
    }

 
}
