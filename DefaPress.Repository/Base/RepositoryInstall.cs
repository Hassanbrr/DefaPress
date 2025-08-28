using DefaPress.Infrastructure.Modules.Article.Implements;
using DefaPress.Infrastructure.Modules.Article.Interfaces;
using DefaPress.Infrastructure.Modules.ArticleCategory.Implements;
using DefaPress.Infrastructure.Modules.ArticleCategory.Interfaces;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DefaPress.Infrastructure.Base
{
    public static class RepositoryInstaller
    {
        public static void InstallRepositories(this IServiceCollection services)
        {

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOffWork, UnitOfWork>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();



        }
    }
}
