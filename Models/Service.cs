using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Service
{
    public int Pkserviceid { get; set; }

    public int Fkbusinessid { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Fkdiscountid { get; set; } = null!;

    public virtual Business Fkbusiness { get; set; } = null!;

    public virtual Discount Fkdiscount { get; set; } = null!;
}
