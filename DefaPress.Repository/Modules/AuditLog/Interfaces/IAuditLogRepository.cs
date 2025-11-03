using DefaPress.Infrastructure.Modules.Base.Interfaces;

namespace DefaPress.Infrastructure.Modules.AuditLog.Interfaces
{
    public interface IAuditLogRepository : IRepository<Domain.AuditLog>
    {
    }
}