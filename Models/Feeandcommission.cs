using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Feeandcommission
{
    [Column("pkFeeID")]
    public string Pkfeeid { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string Description { get; set; } = null!;

    public decimal? Percent { get; set; }

    [Column("FeeType")]
    public string Feetype { get; set; } = null!;

    public string Frequency { get; set; } = null!;
}
