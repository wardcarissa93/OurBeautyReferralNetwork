using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Feeandcommission
{
    public string Pkfeeid { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string Description { get; set; } = null!;

    public decimal? Percent { get; set; }

    public string Feetype { get; set; } = null!;

    public string Frequency { get; set; } = null!;
}
