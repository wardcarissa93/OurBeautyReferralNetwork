namespace OurBeautyReferralNetwork.DataTransferObjects
{
    public class ReferralDTO
    {
        public string PkReferralId { get; set; } = null!;
        public string? FkReferrerCustomerId { get; set; }
        public string? FkReferredCustomerId { get; set; }
        public string? FkReferrerBusinessId { get; set; }
        public string? FkReferredBusinessId { get; set; }
    }
}
