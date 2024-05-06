using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Business
{
    public string PkBusinessId { get; set; } = null!;

    public string BusinessName { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string InsuranceCompany { get; set; } = null!;

    public DateOnly InsuranceExpiryDate { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public bool CommissionPaid { get; set; }

    public string? Description { get; set; }

    public string VerificationDocument { get; set; } = null!;

    public bool IsVerified { get; set; }

    public bool Vip { get; set; }

    public virtual ICollection<Referral> ReferralFkReferredBusinesses { get; set; } = new List<Referral>();

    public virtual ICollection<Referral> ReferralFkReferrerBusinesses { get; set; } = new List<Referral>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
