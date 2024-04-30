namespace OurBeautyReferralNetwork.CustomerModels
{
    public class EditPassword
    {
        public string UserId { get; set; } = null!;
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}