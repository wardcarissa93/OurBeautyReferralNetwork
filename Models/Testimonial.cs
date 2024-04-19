using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Testimonial
{
    public int Pktestimonialid { get; set; }

    public string Fkbusinessid { get; set; }

    public string Description { get; set; } = null!;

    public decimal Rating { get; set; }

    public DateOnly Date { get; set; }

    public virtual Business Fkbusiness { get; set; } = null!;
}
