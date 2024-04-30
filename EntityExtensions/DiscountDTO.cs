using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.EntityExtensions
{
    internal class DiscountDTO : Discount
    {
        public string PkDiscountId { get; set; }
        public decimal Percentage { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}