using System;
using System.Collections.Generic;

namespace ElectionMamagementApplication.Models;

public partial class Party
{
    public int PartyId { get; set; }

    public string PartyName { get; set; } = null!;

    public string LeaderName { get; set; } = null!;

    public DateTime FoundedYear { get; set; }

    public string? PartyDscription { get; set; }

    public String? Email { get; set; }
    public string? Password { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
}
