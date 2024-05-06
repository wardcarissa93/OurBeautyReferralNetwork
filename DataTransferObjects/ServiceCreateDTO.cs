using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.DataTransferObjects
{
    public class ServiceCreateDTO
    {
        public int PkServiceId { get; set; }

        public string Image { get; set; } = null!;

        public string FkBusinessId { get; set; } = null!;

        public string ServiceName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string FkDiscountId { get; set; } = null!;

        public int FkCategoryId { get; set; }

        public decimal? BasePrice { get; set; }

    }
}
