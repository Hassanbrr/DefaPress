using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.Setting.Interfaces;

namespace DefaPress.Infrastructure.Modules.Setting.Implements
{
    public class SettingRepository : Repository<Domain.Setting>, ISettingRepository
    {
        public SettingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}