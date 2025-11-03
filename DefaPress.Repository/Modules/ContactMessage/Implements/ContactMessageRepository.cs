using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.ContactMessage.Interfaces;

namespace DefaPress.Infrastructure.Modules.ContactMessage.Implements
{
    public class ContactMessageRepository : Repository<Domain.ContactMessage>, IContactMessageRepository
    {
        public ContactMessageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}