﻿using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.DataTransferObjects
{
    public class Services
    {
        public int PkServiceId { get; set; }

        public string Image { get; set; } = null!;

        public string FkBusinessId { get; set; } = null!;

        public string ServiceName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string FkDiscountId { get; set; } = null!;

        public int FkCategoryId { get; set; }

        public decimal? BasePrice { get; set; }

        // Navigation property to access Discount's properties
        public virtual Discount FkDiscount { get; set; } = null!;

        // Property to access Discount's Percentage
        public decimal? DiscountPrice => FkDiscount?.Percentage; // This calculates the discount percentage from the related Discount entity

        // Other properties and relationships
        public virtual Business FkBusiness { get; set; } = null!;

        public virtual Category FkCategory { get; set; } = null!;
    }
}
