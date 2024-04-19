using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Testimonial
{
    [Column("pkTestimonialId")]
    public int Pktestimonialid { get; set; }

    [Column("pkBusinessID")]
    public int Fkbusinessid { get; set; }

    public string Description { get; set; } = null!;

    public decimal Rating { get; set; }

    [Column("TestimonialDate")]
    public DateOnly Date { get; set; }

    public virtual Business Fkbusiness { get; set; } = null!;
}
