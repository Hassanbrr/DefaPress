using DefaPress.Infrastructure.Modules.Article.Interfaces;
using DefaPress.Infrastructure.Modules.ArticleCategory.Interfaces;
 

namespace DefaPress.Infrastructure.Modules.Base.Interfaces;

public interface IUnitOffWork
{
    IArticleRepository ArticleRepository { get; }
    IArticleCategoryRepository ArticleCategoryRepository { get; }
    Task SaveChangesAsync( CancellationToken ct = default);

}