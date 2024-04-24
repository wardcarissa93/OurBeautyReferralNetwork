using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Testimonial
{
    public int PkTestimonialId { get; set; }

    public string FkBusinessId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Rating { get; set; }

    public DateOnly TestimonialDate { get; set; }

    public bool Approved { get; set; }

    public virtual Business FkBusiness { get; set; } = null!;
}
