namespace OurBeautyReferralNetwork.CustomerModels
{
    public class EditCustomer
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly Birthdate { get; set; }
        public string Email { get; set; } = null!;
        public bool Vip { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
    }
}