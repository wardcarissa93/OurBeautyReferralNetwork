using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Utilities;

namespace OurBeautyReferralNetwork.Repositories
{
    public class BusinessRepo
    {
        private readonly obrnDbContext _obrnDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JWTUtilities _jwtUtilities;

        public BusinessRepo(obrnDbContext obrnDbContext,
                            UserManager<IdentityUser> userManager,
                            JWTUtilities jWTUtilities)
        {
            _obrnDbContext = obrnDbContext;
            _userManager = userManager;
            _jwtUtilities = jWTUtilities;
        }

        public IEnumerable<Business> GetAllBusinesses()
        {
            IEnumerable<Business> businesses = _obrnDbContext.Businesses.ToList();
            return businesses;
        }

        public async Task<IActionResult> AddBusiness(RegisterBusiness business)
        {
            try
            {
                Business newBusiness = new Business
                {
                    PkBusinessId = business.PkBusinessId,
                    BusinessName = business.BusinessName,
                    Logo = "logo goes here",
                    ContactName = business.ContactName,
                    Address = business.Address,
                    City = business.City,
                    Province = business.Province,
                    PostalCode = business.PostalCode,
                    Email = business.Email,
                    Phone = business.Phone,
                    InsuranceCompany = business.InsuranceCompany,
                    InsuranceExpiryDate = business.InsuranceExpiryDate,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
                    CommissionPaid = false,
                    VerificationDocument = business.VerificationDocument,
                    IsVerified = false,
                };

                _obrnDbContext.Businesses.Add(newBusiness);
                await _obrnDbContext.SaveChangesAsync();

                var user = new IdentityUser
                {
                    UserName = business.PkBusinessId,
                    Email = business.Email
                };

                var result = await _userManager.CreateAsync(user, business.Password);

                if (result.Succeeded)
                {
                    // Generate JWT for the added business
                    var token = _jwtUtilities.GenerateJwtToken(business.Email);

                    return new OkObjectResult(new { Message = "Business added successfully", Token = token });
                }

                return new BadRequestObjectResult(new { Errors = result.Errors });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error adding business: {ex.Message}");
            }
        }
    }
}
