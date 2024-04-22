using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Customer
{
    public string PkCustomerId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }

    public string Phone { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string? Photo { get; set; }

    public string Email { get; set; } = null!;

    public bool Vip { get; set; }

    public bool Confirm18 { get; set; }

    public string? Qr { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Referral> ReferralFkReferredCustomers { get; set; } = new List<Referral>();

    public virtual ICollection<Referral> ReferralFkReferrerCustomers { get; set; } = new List<Referral>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
