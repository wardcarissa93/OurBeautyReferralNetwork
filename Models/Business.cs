using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Business
{

    [Column("pkBusinessID")]
    public int Pkbusinessid { get; set; }

    public string Name { get; set; } = null!;

    public string Logo { get; set; } = null!;

    [Column("ContactName")]
    public string Contactname { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    [Column("PostalCode")]
    public string Postalcode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    [Column("InsuranceCompany")]
    public string Insurancecompany { get; set; } = null!;

    [Column("InsuranceExpiryDate")]
    public DateOnly Insuranceexpirydate { get; set; }

    [Column("RegistrationDate")]

    public DateOnly Registrationdate { get; set; }

    [Column("CommissionPaid")]

    public char Commissionpaid { get; set; }

    public string? Description { get; set; }

    [Column("VerificationDocument")]
    public string Verificationdocument { get; set; } = null!;

    [Column("IsVerified")]

    public char Isverified { get; set; }

    public virtual ICollection<Referral> ReferralFkreferredbusinesses { get; set; } = new List<Referral>();

    public virtual ICollection<Referral> ReferralFkreferrerbusinesses { get; set; } = new List<Referral>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
