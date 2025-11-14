using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain.PollModels
{
    public class PollOption
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }

        public virtual Poll Poll { get; set; }
        public virtual ICollection<PollVote> Votes { get; set; }
    }

}
