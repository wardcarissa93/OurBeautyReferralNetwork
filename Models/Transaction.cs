using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Transaction
{
    public string PkTransactionId { get; set; } = null!;

    public string? FkCustomerId { get; set; }

    public string? FkBusinessId { get; set; }

    public decimal BaseAmount { get; set; }

    public decimal? Tax { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Description { get; set; } = null!;

    public string TransactionTitle { get; set; } = null!;

    public virtual Business? FkBusiness { get; set; }

    public virtual Customer? FkCustomer { get; set; }
}
