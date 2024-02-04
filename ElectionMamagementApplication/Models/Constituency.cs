using System;
using System.Collections.Generic;

namespace ElectionMamagementApplication.Models;

public partial class Constituency
{
    public int ConstituencyId { get; set; }

    public string ConstituencyName { get; set; } = null!;

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Voter> Voters { get; set; } = new List<Voter>();

    public virtual ICollection<ElectionsResult> Elections { get; set; } = new List<ElectionsResult>();
}
