namespace ElectionMamagementApplication.ModelView
{
    public class VoterView
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

        public bool IsVoted { get; set; }

        public long AadharNo { get; set; }
    }
    public class VoterViewOut
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int ConstituencyId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsVoted { get; set; }

        public long AadharNo { get; set; }
    }
}
