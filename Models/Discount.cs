using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Discount
{
    [Column("pkDiscountID")]
    public string Pkdiscountid { get; set; } = null!;

    public string Image { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string Description { get; set; } = null!;

    public decimal? Percent { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
