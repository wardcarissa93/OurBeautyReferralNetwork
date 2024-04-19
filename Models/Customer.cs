using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Customer
{
    [Column("pkCustomerID")]
    public required string Pkcustomerid { get; set; }

    [Column("FirstName")]
    public string Firstname { get; set; } = null!;

    [Column("LastName")]
    public string Lastname { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    [Column("PostalCode")]
    public string? Postalcode { get; set; }

    public string Phone { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string? Photo { get; set; }

    public string Email { get; set; } = null!;

    [Column("VIP")]
    public char Vip { get; set; }

    public char Confirm18 { get; set; }

    [Column("QR")]
    public string? Qr { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Referral> ReferralFkreferredcustomers { get; set; } = new List<Referral>();

    public virtual ICollection<Referral> ReferralFkreferrercustomers { get; set; } = new List<Referral>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
