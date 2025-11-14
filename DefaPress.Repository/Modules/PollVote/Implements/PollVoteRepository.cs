using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Implements;
using DefaPress.Infrastructure.Modules.PollVote.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Infrastructure.Modules.PollVote.Implements
{
    public class PollVoteRepository : Repository<Domain.PollModels.PollVote>, IPollVoteRepository
    {
        public PollVoteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
