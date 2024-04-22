using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Reward
{
    public int PkRewardId { get; set; }

    public int FkReferralId { get; set; }

    public decimal RewardAmount { get; set; }

    public DateOnly IssueDate { get; set; }

    public virtual Referral FkReferral { get; set; } = null!;
}
