namespace ElectionMamagementApplication.ModelView
{
    public class ElectionResultView
    {
        public int ElectionId { get; set; }

        public int CandidateId { get; set; }

        public long TotalVotes { get; set; }

        public decimal PercentageVotes { get; set; }

        public int ConstituencyId { get; set; }
    }
    public class ElectionResultViewOut
    {
        public int ResultId { get; set; }
        public int ElectionId { get; set; }

        public int CandidateId { get; set; }

        public long TotalVotes { get; set; }

        public decimal PercentageVotes { get; set; }

        public int ConstituencyId { get; set; }
    }
   
}
