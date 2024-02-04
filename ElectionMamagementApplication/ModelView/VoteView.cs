namespace ElectionMamagementApplication.ModelView
{
    public class VoteView
    {
        public int VoterId { get; set; }

        public int CandidateId { get; set; }

        public int ElectionId { get; set; }

        public DateTime VoteTimeStamp { get; set; }
    }
}
