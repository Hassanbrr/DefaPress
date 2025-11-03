using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.AuditLog.Interfaces;

namespace DefaPress.Infrastructure.Modules.AuditLog.Implements
{
    public class AuditLogRepository : Repository<Domain.AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}