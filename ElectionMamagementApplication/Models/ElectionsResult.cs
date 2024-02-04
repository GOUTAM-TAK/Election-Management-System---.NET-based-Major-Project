using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ElectionMamagementApplication.Models;

public partial class ElectionsResult
{
    public int ResultId { get; set; }

    public int ElectionId { get; set; }

    public int CandidateId { get; set; }

    public long TotalVotes { get; set; }

    public decimal PercentageVotes { get; set; }

    public int ConstituencyId { get; set; }

    [JsonIgnore]
    public virtual Constituency Constituency { get; set; }
    [JsonIgnore]
    public virtual Candidate Candidate { get; set; } = null!;
    [JsonIgnore]
    public virtual Election Election { get; set; } = null!;
}
