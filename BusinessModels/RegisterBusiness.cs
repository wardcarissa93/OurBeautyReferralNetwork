namespace OurBeautyReferralNetwork.BusinessModels;

public partial class RegisterBusiness
{
    public string PkBusinessId { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string ContactName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string InsuranceCompany { get; set; } = null!;
    public DateOnly InsuranceExpiryDate { get; set; }
    public string VerificationDocument { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
