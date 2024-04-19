using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Review
{
    public int Pkreviewid { get; set; }

    public string Fkbusinessid { get; set; }

    public string Fkcustomerid { get; set; }

    public string Description { get; set; } = null!;

    public decimal Rating { get; set; }

    public DateOnly Date { get; set; }

    public string? Image { get; set; }

    public string? Provider { get; set; }

    public virtual Business Fkbusiness { get; set; } = null!;

    public virtual Customer Fkcustomer { get; set; } = null!;
}
