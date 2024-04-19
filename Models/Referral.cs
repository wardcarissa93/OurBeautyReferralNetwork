using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Referral
{
    [Column("pkReferralID")]
    public int Pkreferralid { get; set; }

    [Column("fkReferrerCustomerID")]
    public int? Fkreferrercustomerid { get; set; }

    [Column("fkReferredCustomerID")]
    public int? Fkreferredcustomerid { get; set; }

    [Column("fkReferrerBusinessID")]
    public int? Fkreferrerbusinessid { get; set; }

    [Column("fkReferredBusinessID")]
    public int? Fkreferredbusinessid { get; set; }

    [Column("pkBusReferralDateinessID")]
    public DateOnly Referraldate { get; set; }

    public string Status { get; set; } = null!;

    [Column("ReferredType")]
    public string Referredtype { get; set; } = null!;

    public virtual Business? Fkreferredbusiness { get; set; }

    public virtual Customer? Fkreferredcustomer { get; set; }

    public virtual Business? Fkreferrerbusiness { get; set; }

    public virtual Customer? Fkreferrercustomer { get; set; }

    public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();
}
