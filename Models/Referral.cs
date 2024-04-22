using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Referral
{
    public int PkReferralId { get; set; }

    public string? FkReferrerCustomerId { get; set; }

    public string? FkReferredCustomerId { get; set; }

    public string? FkReferrerBusinessId { get; set; }

    public string? FkReferredBusinessId { get; set; }

    public DateOnly ReferralDate { get; set; }

    public string Status { get; set; } = null!;

    public string ReferredType { get; set; } = null!;

    public virtual Business? FkReferredBusiness { get; set; }

    public virtual Customer? FkReferredCustomer { get; set; }

    public virtual Business? FkReferrerBusiness { get; set; }

    public virtual Customer? FkReferrerCustomer { get; set; }

    public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();
}
