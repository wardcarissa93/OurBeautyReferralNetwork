using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Category
{
    public int PkCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
