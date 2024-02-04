using System;
using System.Collections.Generic;

namespace ElectionMamagementApplication.Models;

public partial class Vote
{
    public int VoteId { get; set; }

    public int VoterId { get; set; }

    public int CandidateId { get; set; }

    public int ElectionId { get; set; }

    public DateTime VoteTimeStamp { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Election Election { get; set; } = null!;

    public virtual Voter Voter { get; set; } = null!;
}
