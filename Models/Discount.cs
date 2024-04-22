using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Discount
{
    public string PkDiscountId { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal? Percentage { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
