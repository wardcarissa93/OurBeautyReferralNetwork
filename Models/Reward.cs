using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Reward
{
    [Column("pkRewardID")]
    public int Pkrewardid { get; set; }

    [Column("fkReferralID")]
    public int Fkreferralid { get; set; }

    [Column("RewardAmount")]
    public decimal Rewardamount { get; set; }

    [Column("IssueDate")]
    public DateOnly Issuedate { get; set; }

    public virtual Referral Fkreferral { get; set; } = null!;
}
