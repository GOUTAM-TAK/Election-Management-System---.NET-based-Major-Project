namespace ElectionMamagementApplication.ModelView
{
    public class PartyView
    {
        public string PartyName { get; set; } = null!;

        public string LeaderName { get; set; } = null!;

        public DateTime FoundedYear { get; set; }

        public string? PartyDscription { get; set; }
        public String? Email { get; set; }
        public string? Password { get; set; }
    }
}
