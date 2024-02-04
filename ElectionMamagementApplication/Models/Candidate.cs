using System;
using System.Collections.Generic;

namespace ElectionMamagementApplication.Models;

public partial class Candidate
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

    public int? PartyId { get; set; }

    public int ElectionId { get; set; }

    public virtual Election Election { get; set; }

    public virtual Constituency? Constituency { get; set; } 

    public virtual ICollection<ElectionsResult> ElectionsResults { get; set; } = new List<ElectionsResult>();

    public virtual Party? Party { get; set; }

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
