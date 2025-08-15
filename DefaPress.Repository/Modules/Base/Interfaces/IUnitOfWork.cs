using DefaPress.Repository.Modules.ArticleCategory.Interfaces;
using DefaPress.Repository.Modules.ArticlePost.Interfaces;

namespace DefaPress.Repository.Modules.Base.Interfaces;

public interface IUnitOffWork
{
    IArticleRepository ArticleRepository { get; }
    IArticleCategoryRepository ArticleCategoryRepository { get; }
    Task SaveChangesAsync( CancellationToken ct = default);

}