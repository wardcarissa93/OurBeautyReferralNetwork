using Microsoft.AspNetCore.Identity;

namespace OurBeautyReferralNetwork.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Custom property for the user's first name
        public string FirstName { get; set; }

        // Custom property for the user's last name
        public string LastName { get; set; }

        // Custom property for a profile picture URL
        public string? ProfilePictureUrl { get; set; }

        // Method returns full name
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
