using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Service
{
    [Column("pkServiceID")]
    public int Pkserviceid { get; set; }

    [Column("fkBusinessID")]
    public int Fkbusinessid { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column("fkDiscountID")]
    public string Fkdiscountid { get; set; } = null!;

    public virtual Business Fkbusiness { get; set; } = null!;

    public virtual Discount Fkdiscount { get; set; } = null!;
}
