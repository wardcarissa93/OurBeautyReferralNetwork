using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Discount
{
    public string PkDiscountId { get; set; } = null!;

    public decimal? Percentage { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
