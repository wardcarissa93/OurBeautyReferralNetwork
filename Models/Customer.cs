using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Customer
{
    public required string Pkcustomerid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? Postalcode { get; set; }

    public string Phone { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime Birthdate { get; set; }

    public string? Photo { get; set; }

    public string Email { get; set; } = null!;

    public char Vip { get; set; }

    public char Confirm18 { get; set; }

    public string? Qr { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Referral> ReferralFkreferredcustomers { get; set; } = new List<Referral>();

    public virtual ICollection<Referral> ReferralFkreferrercustomers { get; set; } = new List<Referral>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
