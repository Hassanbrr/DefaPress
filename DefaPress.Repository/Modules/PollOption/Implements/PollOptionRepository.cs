using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements; 
using DefaPress.Infrastructure.Modules.PollOption.Interfaces;

namespace DefaPress.Infrastructure.Modules.PollOption.Implements
{
    public class PollOptionRepository : Repository<Domain.PollModels.PollOption>, IPollOptionRepository
    {
        public PollOptionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}