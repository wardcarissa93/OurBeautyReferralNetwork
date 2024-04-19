using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Referral
{
    public int Pkreferralid { get; set; }

    public string? Fkreferrercustomerid { get; set; }

    public string? Fkreferredcustomerid { get; set; }

    public string? Fkreferrerbusinessid { get; set; }

    public string? Fkreferredbusinessid { get; set; }

    public DateOnly Referraldate { get; set; }

    public string Status { get; set; } = null!;

    public string Referredtype { get; set; } = null!;

    public virtual Business? Fkreferredbusiness { get; set; }

    public virtual Customer? Fkreferredcustomer { get; set; }

    public virtual Business? Fkreferrerbusiness { get; set; }

    public virtual Customer? Fkreferrercustomer { get; set; }

    public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();
}
