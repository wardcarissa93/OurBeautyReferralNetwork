using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Service
{
    public int PkServiceId { get; set; }

    public string Image { get; set; } = null!;

    public string FkBusinessId { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string FkDiscountId { get; set; } = null!;

    public int FkCategoryId { get; set; }

    public decimal? BasePrice { get; set; }

    public virtual Business FkBusiness { get; set; } = null!;

    public virtual Category FkCategory { get; set; } = null!;

    public virtual Discount FkDiscount { get; set; } = null!;
}
