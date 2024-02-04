using System;
using System.Collections.Generic;

namespace ElectionMamagementApplication.Models;

public partial class Election
{
    public int ElectionId { get; set; }

    public string ElectionName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string ElectionStatus { get; set; } = null!;

    public virtual ICollection<ElectionsResult> ElectionsResults { get; set; } = new List<ElectionsResult>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

}
