using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.DataTransferObjects
{
    public class ReviewDTO
    {
        public int PkReviewId { get; set; }

        public string FkBusinessId { get; set; } = null!;

        public string FkCustomerId { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Rating { get; set; }

        public DateOnly ReviewDate { get; set; }

        public string? Image { get; set; }

        public string? Provider { get; set; }

        //public virtual Business FkBusiness { get; set; } = null!;

        //public virtual Customer FkCustomer { get; set; } = null!;
    }
}
