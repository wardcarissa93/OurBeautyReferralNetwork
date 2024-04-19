using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Review
{
    [Column("pkReviewId")]
    public int Pkreviewid { get; set; }

    [Column("fkBusinessID")]
    public int Fkbusinessid { get; set; }

    [Column("fkCustomerID")]
    public int Fkcustomerid { get; set; }

    public string Description { get; set; } = null!;

    public decimal Rating { get; set; }

    public DateOnly Date { get; set; }

    public string? Image { get; set; }

    public string? Provider { get; set; }

    public virtual Business Fkbusiness { get; set; } = null!;

    public virtual Customer Fkcustomer { get; set; } = null!;
}
