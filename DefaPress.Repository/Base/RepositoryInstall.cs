using DefaPress.Infrastructure.Modules.Article.Implements;
using DefaPress.Infrastructure.Modules.Article.Interfaces;
using DefaPress.Infrastructure.Modules.ArticleCategory.Implements;
using DefaPress.Infrastructure.Modules.ArticleCategory.Interfaces;
using DefaPress.Infrastructure.Modules.AuditLog.Implements;
using DefaPress.Infrastructure.Modules.AuditLog.Interfaces;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
using DefaPress.Infrastructure.Modules.Comment.Implements;
using DefaPress.Infrastructure.Modules.Comment.Interfaces;
using DefaPress.Infrastructure.Modules.ContactMessage.Implements;
using DefaPress.Infrastructure.Modules.ContactMessage.Interfaces;
using DefaPress.Infrastructure.Modules.MediaFile.Implements;
using DefaPress.Infrastructure.Modules.MediaFile.Interfaces;
using DefaPress.Infrastructure.Modules.NewsletterSubscriber.Implements;
using DefaPress.Infrastructure.Modules.NewsletterSubscriber.Interfaces;
using DefaPress.Infrastructure.Modules.Poll.Implements;
using DefaPress.Infrastructure.Modules.Poll.Interfaces;
using DefaPress.Infrastructure.Modules.PollOption.Implements;
using DefaPress.Infrastructure.Modules.PollOption.Interfaces;
using DefaPress.Infrastructure.Modules.PollVote.Implements;
using DefaPress.Infrastructure.Modules.PollVote.Interfaces;
using DefaPress.Infrastructure.Modules.Setting.Implements;
using DefaPress.Infrastructure.Modules.Setting.Interfaces;
using DefaPress.Infrastructure.Modules.Tag.Implements;
using DefaPress.Infrastructure.Modules.Tag.Interfaces;
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
 
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            services.AddScoped<INewsletterSubscriberRepository, NewsletterSubscriberRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IPollRepository, PollRepository>();
            services.AddScoped<IPollOptionRepository, PollOptionRepository>();
            services.AddScoped<IPollVoteRepository, PollVoteRepository>();


        }
    }
}
