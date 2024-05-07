using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Subscription
{
    public int PkSubscriptionId { get; set; }

    public string FkBusinessId { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Description { get; set; } = null!;

    public string SubscriptionTitle { get; set; } = null!;

    public string FeeType { get; set; } = null!;

    public string Frequency { get; set; } = null!;

    public DateOnly NextPaymentDay { get; set; }

    public bool IsActive { get; set; }

    public string? FkTransactionId { get; set; }

    public virtual Business FkBusiness { get; set; } = null!;
}
