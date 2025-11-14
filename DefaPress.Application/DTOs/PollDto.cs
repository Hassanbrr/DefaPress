using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{
    // PollDto.cs
    public class PollDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; }
        public List<PollOptionDto> Options { get; set; }
        public int TotalVotes { get; set; }
        public bool HasVoted { get; set; }
    }

    // PollOptionDto.cs
    public class PollOptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
        public bool IsSelected { get; set; }
    }

    // CreatePollDto.cs
    public class CreatePollDto
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int? DurationDays { get; set; }
    }
    public class VoteRequestDto
    {
        public int PollId { get; set; }
        public int OptionId { get; set; }
    }
}
