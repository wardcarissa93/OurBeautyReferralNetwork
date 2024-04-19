using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Business
{
    public required string Pkbusinessid { get; set; }

    public string Name { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string Contactname { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string Postalcode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Insurancecompany { get; set; } = null!;

    public DateOnly Insuranceexpirydate { get; set; }

    public DateOnly Registrationdate { get; set; }

    public char Commissionpaid { get; set; }

    public string? Description { get; set; }

    public string Verificationdocument { get; set; } = null!;

    public char Isverified { get; set; }

    public virtual ICollection<Referral> ReferralFkreferredbusinesses { get; set; } = new List<Referral>();

    public virtual ICollection<Referral> ReferralFkreferrerbusinesses { get; set; } = new List<Referral>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
