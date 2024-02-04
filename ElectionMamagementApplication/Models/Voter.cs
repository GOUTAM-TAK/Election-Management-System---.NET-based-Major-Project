using System;
using System.Collections.Generic;

namespace ElectionMamagementApplication.Models;

public partial class Voter
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

    public virtual Constituency Constituency { get; set; }

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
