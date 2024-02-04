namespace ElectionMamagementApplication.ModelView
{
    public class CandidateView
    {
      

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string Email { get; set; } = null!;
        
        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int ConstituencyId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? PartyId { get; set; }

        public int ElectionId { get; set; }
    }
}
