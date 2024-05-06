namespace OurBeautyReferralNetwork.DataTransferObjects
{
    public class EditBusinessDTO
    {
        public string ContactName { get; set; } = null!;
        public string BusinessName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Description { get; set; }
        public string Logo { get; set; } = null!;
        public string InsuranceCompany { get; set; } = null!;
        public DateOnly InsuranceExpiryDate { get; set; }
        public string VerificationDocument { get; set; } = null!;
    }
}
