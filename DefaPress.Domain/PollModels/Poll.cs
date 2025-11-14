namespace DefaPress.Domain.PollModels
{
    public class Poll
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        // Navigation Properties
        public virtual ICollection<PollOption> Options { get; set; }
        public virtual ICollection<PollVote> Votes { get; set; }

       
    }
}