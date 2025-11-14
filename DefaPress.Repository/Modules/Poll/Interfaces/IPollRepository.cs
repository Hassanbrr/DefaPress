using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaPress.Infrastructure.Modules.Base.Interfaces;

namespace DefaPress.Infrastructure.Modules.Poll.Interfaces
{
    public interface IPollRepository : IRepository<Domain.PollModels.Poll>
    {
    }
}
