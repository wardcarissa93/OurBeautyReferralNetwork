using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Utilities;

namespace OurBeautyReferralNetwork.Repositories
{
    public class BusinessRepo
    {
        private readonly obrnDbContext _obrnDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JWTUtilities _jwtUtilities;
        private readonly ReferralRepo _referralRepo;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserRepo _userRepo;
        private readonly ApplicationDbContext _context;

        public BusinessRepo(obrnDbContext obrnDbContext,
                            UserManager<IdentityUser> userManager,
                            JWTUtilities jWTUtilities,
                            ReferralRepo referralRepo,
                            SignInManager<IdentityUser> signInManager,
                            UserRepo userRepo,
                            ApplicationDbContext context)
        {
            _obrnDbContext = obrnDbContext;
            _userManager = userManager;
            _jwtUtilities = jWTUtilities;
            _referralRepo = referralRepo;
            _signInManager = signInManager;
            _userRepo = userRepo;
            _context = context;
        }

        public IEnumerable<Business> GetAllBusinesses()
        {
            IEnumerable<Business> businesses = _obrnDbContext.Businesses.ToList();
            return businesses;
        }

        public async Task<IActionResult> GetBusinessById(string businessId)
        {
            try
            {
                Business business = await _obrnDbContext.Businesses.FirstOrDefaultAsync(c => c.PkBusinessId == businessId);
                if (business != null)
                {
                    return new OkObjectResult(business);
                }
                else
                {
                    return new NotFoundObjectResult("Business not found");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error getting business: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetBusinessByEmail(string email)
        {
            try
            {
                Business business = await _obrnDbContext.Businesses.FirstOrDefaultAsync(b => b.Email == email);
                if (business != null)
                {
                    return new OkObjectResult(business);
                }
                else
                {
                    return new NotFoundObjectResult("Business not found");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error getting business: {ex.Message}");
            }
        }

        public async Task<IActionResult> AddBusiness(RegisterBusinessDTO business)
        {
            try
            {
                using (var dbContext = new obrnDbContext())
                {
                    var isBusinessUsernameAvailable = await IsBusinessUsernameAvailable(business.PkBusinessId);
                    if (!isBusinessUsernameAvailable)
                    {
                        return new BadRequestObjectResult("Username unavailable. Please enter a different username.");
                    }

                    Business newBusiness = CreateNewBusiness(business);
                    dbContext.Businesses.Add(newBusiness);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine("New business added");

                    var user = await _userRepo.AddNewUser(business.PkBusinessId, business.Email, business.Password, "business");

                    var token = await _jwtUtilities.GenerateJwtToken(business.Email);

                    var referralResult = await HandleBusinessReferral(business, user);
                    if (referralResult is OkObjectResult referralOkResult)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        Console.WriteLine("User logged in");
                        return new OkObjectResult(new { Message = "Business added successfully", Token = token, ReferralId = referralOkResult.Value });
                    }
                    return referralResult;
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error adding business: {ex.Message}");
            }
        }

        public async Task<IActionResult> EditBusiness(EditBusinessDTO business)
        {
            try
            {
                // Check if the business exists in the database
                Business existingBusiness = await _obrnDbContext.Businesses.FirstOrDefaultAsync(b => b.Email == business.Email);
                if (existingBusiness == null)
                {
                    return new NotFoundObjectResult("Business not found");
                }

                // Update the business's properties
                existingBusiness.ContactName = business.ContactName;
                existingBusiness.BusinessName = business.BusinessName;
                existingBusiness.Address = business.Address;
                existingBusiness.City = business.City;
                existingBusiness.Province = business.Province;
                existingBusiness.PostalCode = business.PostalCode;
                existingBusiness.Email = business.Email;
                existingBusiness.Description = business.Description;
                existingBusiness.Logo = business.Logo;
                existingBusiness.InsuranceCompany = business.InsuranceCompany;
                existingBusiness.InsuranceExpiryDate = business.InsuranceExpiryDate;
                existingBusiness.VerificationDocument = business.VerificationDocument;

                // Save changes to the database
                await _obrnDbContext.SaveChangesAsync();

                return new OkObjectResult("Business updated successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error editing customer: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteBusiness(string businessId)
        {
            try
            {
                // Find the business by ID
                Business business = await _obrnDbContext.Businesses.FirstOrDefaultAsync(c => c.PkBusinessId == businessId);
                if (business == null)
                {
                    return new NotFoundObjectResult("Business not found");
                }

                // Find the corresponding AspNetUser by email
                var user = await _userManager.FindByEmailAsync(business.Email);
                if (user == null)
                {
                    return new NotFoundObjectResult("User not found");
                }

                // Find and delete the business's referral code
                Referral referralCode = await _obrnDbContext.Referrals.FirstOrDefaultAsync(r => r.FkReferredBusinessId == businessId);
                _obrnDbContext.Referrals.Remove(referralCode);

                // Find and delete any referrals made by this business
                var referrals = _obrnDbContext.Referrals.Where(r => r.FkReferredBusinessId != businessId);
                if (referrals.Any())
                {
                    _obrnDbContext.Referrals.RemoveRange(referrals);
                }

                // Delete the business
                _obrnDbContext.Businesses.Remove(business);
                await _obrnDbContext.SaveChangesAsync();

                // Delete the AspNetUser
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Handle delete user error if needed
                    return new BadRequestObjectResult("Error deleting user");
                }

                return new OkObjectResult("Business and associated user deleted successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error deleting business: {ex.Message}");
            }
        }

        

        private async Task<bool> IsBusinessUsernameAvailable(string username)
        {
            obrnDbContext dbContext = new obrnDbContext();
            Business existingBusiness = await _obrnDbContext.Businesses.FirstOrDefaultAsync(b => b.PkBusinessId == username);
            return existingBusiness == null;
        }

        private Business CreateNewBusiness(RegisterBusinessDTO business)
        {
            return new Business
            {
                PkBusinessId = business.PkBusinessId,
                BusinessName = business.BusinessName,
                Logo = business.Logo,
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
                IsVerified = false
            };
        }

        private async Task<IActionResult> HandleBusinessReferral(RegisterBusinessDTO business, IdentityUser user)
        {
            if (business.FkReferralId != null)
            {
                ReferralRepo referralRepo = new ReferralRepo(_context, _obrnDbContext);
                var referralType = await referralRepo.GetReferralTypeById(business.FkReferralId);
                switch (referralType.ToString())
                {
                    case "C":
                        return await HandleBusinessReferralByCustomer(business, user);
                    case "B":
                        return await HandleBusinessReferralByBusiness(business, user);
                    default:
                        return await HandleDefaultBusinessReferral(business);
                }
            }
            else
            {
                return await HandleDefaultBusinessReferral(business);
            }
        }

        private async Task<IActionResult> HandleBusinessReferralByCustomer(RegisterBusinessDTO business, IdentityUser user)
        {
            ReferralRepo referralRepo1 = new ReferralRepo(_context, _obrnDbContext);
            var referrerCustomerId = await referralRepo1.GetFkReferredCustomerId(business.FkReferralId);
            ReferralDTO referralDTO = new ReferralDTO
            {
                FkReferrerCustomerId = referrerCustomerId.ToString(),
                FkReferredBusinessId = business.PkBusinessId,
            };

            ReferralRepo referralRepo2 = new ReferralRepo(_context, _obrnDbContext);
            var referralResult = await referralRepo2.CreateReferralCodeForBusiness(referralDTO);
            if (referralResult is OkObjectResult referralOkResult)
            {
                Console.WriteLine("Referral code created");
                return new OkObjectResult(new { Message = "Referral completed successfully", ReferralId = referralOkResult.Value });
            }
            return referralResult;
        }

        private async Task<IActionResult> HandleBusinessReferralByBusiness(RegisterBusinessDTO business, IdentityUser user)
        {
            ReferralRepo referralRepo1 = new ReferralRepo(_context, _obrnDbContext);
            var referrerBusinessId = await referralRepo1.GetFkReferredBusinessId(business.FkReferralId);
            ReferralDTO referralDTO = new ReferralDTO
            {
                FkReferrerBusinessId = referrerBusinessId.ToString(),
                FkReferredBusinessId = business.PkBusinessId,
            };

            ReferralRepo referralRepo2 = new ReferralRepo(_context, _obrnDbContext);
            var referralResult = await referralRepo2.CreateReferralCodeForBusiness(referralDTO);
            if (referralResult is OkObjectResult referralOkResult)
            {
                Console.WriteLine("Referral code created");
                return new OkObjectResult(new { Message = "Referral completed successfully", ReferralId = referralOkResult.Value });
            }
            return referralResult;
        }

        private async Task<IActionResult> HandleDefaultBusinessReferral(RegisterBusinessDTO business)
        {
            ReferralRepo referralRepo = new ReferralRepo(_context, _obrnDbContext);
            ReferralDTO referralDTO = new ReferralDTO
            {
                FkReferredBusinessId = business.PkBusinessId
            };

            var referralResult = await referralRepo.CreateReferralCodeForBusiness(referralDTO);
            if (referralResult is OkObjectResult referralOkResult)
            {
                Console.WriteLine("Referral code created");
                return new OkObjectResult(new { Message = "Referral completed successfully", ReferralId = referralOkResult.Value });
            }
            return referralResult;
        }
    }
}
