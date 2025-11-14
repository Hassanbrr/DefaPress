using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.Poll.Interfaces;

namespace DefaPress.Infrastructure.Modules.Poll.Implements
{
    internal class PollRepository : Repository<Domain.PollModels.Poll> , IPollRepository
    {
        public PollRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
