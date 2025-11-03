using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Article.Interfaces;
using DefaPress.Infrastructure.Modules.ArticleCategory.Interfaces;
using DefaPress.Infrastructure.Modules.AuditLog.Interfaces;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
using DefaPress.Infrastructure.Modules.Comment.Interfaces;
using DefaPress.Infrastructure.Modules.ContactMessage.Interfaces;
using DefaPress.Infrastructure.Modules.MediaFile.Interfaces;
using DefaPress.Infrastructure.Modules.NewsletterSubscriber.Interfaces;
using DefaPress.Infrastructure.Modules.Setting.Interfaces;
using DefaPress.Infrastructure.Modules.Tag.Interfaces;

namespace DefaPress.Infrastructure.Modules.Base.Implements
{
    public class UnitOfWork : IUnitOffWork
    {
        private readonly ApplicationDbContext _context;

        public IArticleRepository ArticleRepository { get; private set; }
        public IArticleCategoryRepository ArticleCategoryRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public IContactMessageRepository ContactMessageRepository { get; private set; }
        public IMediaFileRepository MediaFileRepository { get; private set; }
        public INewsletterSubscriberRepository NewsletterSubscriberRepository { get; private set; }
        public ISettingRepository SettingRepository { get; private set; }
        public ITagRepository TagRepository { get; private set; }
        public IAuditLogRepository AuditLogRepository { get; private set; }

        public UnitOfWork(
            ApplicationDbContext context,
            IArticleRepository articleRepository,
            IArticleCategoryRepository articleCategoryRepository,
            ICommentRepository commentRepository,
            IContactMessageRepository contactMessageRepository,
            IMediaFileRepository mediaFileRepository,
            INewsletterSubscriberRepository newsletterSubscriberRepository,
            ISettingRepository settingRepository,
            ITagRepository tagRepository,
            IAuditLogRepository auditLogRepository)
        {
            _context = context;
            ArticleRepository = articleRepository;
            ArticleCategoryRepository = articleCategoryRepository;
            CommentRepository = commentRepository;
            ContactMessageRepository = contactMessageRepository;
            MediaFileRepository = mediaFileRepository;
            NewsletterSubscriberRepository = newsletterSubscriberRepository;
            SettingRepository = settingRepository;
            TagRepository = tagRepository;
            AuditLogRepository = auditLogRepository;
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}