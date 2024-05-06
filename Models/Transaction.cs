using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Transaction
{
    public int PkTransactionId { get; set; }

    public string? FkCustomerId { get; set; }

    public string? FkBusinessId { get; set; }

    public decimal BaseAmount { get; set; }

    public decimal? Tax { get; set; }

    public decimal TotalAmount { get; set; }

    public DateOnly TransactionDate { get; set; }

    public string Description { get; set; } = null!;

    public string TransactionTitle { get; set; } = null!;

    public int FkSubscriptionId { get; set; }

    public virtual Business? FkBusiness { get; set; }

    public virtual Customer? FkCustomer { get; set; }

    public virtual Subscription FkSubscription { get; set; } = null!;

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
