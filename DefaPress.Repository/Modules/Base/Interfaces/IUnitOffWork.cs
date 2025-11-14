using DefaPress.Infrastructure.Modules.Article.Interfaces;
using DefaPress.Infrastructure.Modules.ArticleCategory.Interfaces;
using DefaPress.Infrastructure.Modules.AuditLog.Interfaces;
using DefaPress.Infrastructure.Modules.Comment.Interfaces;
using DefaPress.Infrastructure.Modules.ContactMessage.Interfaces;
using DefaPress.Infrastructure.Modules.MediaFile.Interfaces;
using DefaPress.Infrastructure.Modules.NewsletterSubscriber.Interfaces;
using DefaPress.Infrastructure.Modules.Poll.Interfaces;
using DefaPress.Infrastructure.Modules.PollOption.Interfaces;
using DefaPress.Infrastructure.Modules.PollVote.Interfaces;
using DefaPress.Infrastructure.Modules.Setting.Interfaces;
using DefaPress.Infrastructure.Modules.Tag.Interfaces;

namespace DefaPress.Infrastructure.Modules.Base.Interfaces;

public interface IUnitOffWork
{
    // Existing repositories...
    IArticleRepository ArticleRepository { get; }
    IArticleCategoryRepository ArticleCategoryRepository { get; }
    ICommentRepository CommentRepository { get; }
    IContactMessageRepository ContactMessageRepository { get; }
    IMediaFileRepository MediaFileRepository { get; }
    INewsletterSubscriberRepository NewsletterSubscriberRepository { get; }
    ISettingRepository SettingRepository { get; }
    ITagRepository TagRepository { get; }
    IAuditLogRepository AuditLogRepository { get; }
    IPollRepository PollRepository { get; }

    // Add these new repositories
    IPollOptionRepository PollOptionRepository { get; }
    IPollVoteRepository PollVoteRepository { get; }

    Task SaveChangesAsync(CancellationToken ct = default);
}