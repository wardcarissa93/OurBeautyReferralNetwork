using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class FeeAndCommission
{
    public string PkFeeId { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Percentage { get; set; }

    public string FeeType { get; set; } = null!;

    public string Frequency { get; set; } = null!;
}
