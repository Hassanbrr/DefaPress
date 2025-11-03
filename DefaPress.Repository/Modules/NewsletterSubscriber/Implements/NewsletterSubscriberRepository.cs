using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.NewsletterSubscriber.Interfaces;

namespace DefaPress.Infrastructure.Modules.NewsletterSubscriber.Implements
{
    public class NewsletterSubscriberRepository : Repository<Domain.NewsletterSubscriber>, INewsletterSubscriberRepository
    {
        public NewsletterSubscriberRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}