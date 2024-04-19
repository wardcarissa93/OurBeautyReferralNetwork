using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Reward
{
    public int Pkrewardid { get; set; }

    public int Fkreferralid { get; set; }

    public decimal Rewardamount { get; set; }

    public DateOnly Issuedate { get; set; }

    public virtual Referral Fkreferral { get; set; } = null!;
}
