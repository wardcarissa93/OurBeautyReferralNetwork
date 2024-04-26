namespace OurBeautyReferralNetwork.DataTransferObjects
{
    public class TestimonialDTO
    {
        public string FkBusinessId { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Rating { get; set; }
        public bool Approved { get; set; }

    }
}
