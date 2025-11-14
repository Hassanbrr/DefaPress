using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain.PollModels
{
    public class PollVote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public int OptionId { get; set; }
        public string? VoterIp { get; set; }
        public string? VoterUserId { get; set; } // اگر سیستم کاربران دارید
        public DateTime VotedAt { get; set; }

        public virtual Poll Poll { get; set; }
        public virtual PollOption Option { get; set; }
    }
}
