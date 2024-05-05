using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public BusinessRepo(obrnDbContext obrnDbContext,
                            UserManager<IdentityUser> userManager,
                            JWTUtilities jWTUtilities,
                            ReferralRepo referralRepo,
                            SignInManager<IdentityUser> signInManager)
        {
            _obrnDbContext = obrnDbContext;
            _userManager = userManager;
            _jwtUtilities = jWTUtilities;
            _referralRepo = referralRepo;
            _signInManager = signInManager;
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
                    Business existingBusiness = await dbContext.Businesses.FirstOrDefaultAsync(b => b.PkBusinessId == business.PkBusinessId);
                    if (existingBusiness != null)
                    {
                        return new BadRequestObjectResult("Username unavailable. Please enter a different username.");
                    }

                    Business newBusiness = new Business
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

                    dbContext.Businesses.Add(newBusiness);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine("New business added");

                    var user = new IdentityUser
                    {
                        UserName = business.PkBusinessId,
                        Email = business.Email
                    };

                    var addUserResult = await _userManager.CreateAsync(user, business.Password);

                    if (addUserResult.Succeeded)
                    {
                        Console.WriteLine("New user added");
                        var addUserRoleResult = await _userManager.AddToRoleAsync(user, "business");

                        if (addUserRoleResult.Succeeded)
                        {
                            Console.WriteLine("Business role added to new user");
                            var token = _jwtUtilities.GenerateJwtToken(business.Email);

                            if (business.FkReferralId != null)
                            {
                                var referralType = await _referralRepo.GetReferralTypeById(business.FkReferralId);
                                if (referralType.ToString() == "C")
                                {
                                    var referrerCustomerId = await _referralRepo.GetFkReferredCustomerId(business.FkReferralId);
                                    ReferralDTO referralDTO = new ReferralDTO
                                    {
                                        FkReferrerCustomerId = referrerCustomerId.ToString(),
                                        FkReferredBusinessId = business.PkBusinessId
                                    };

                                    var referralResult = await _referralRepo.CreateReferralCodeForBusiness(referralDTO);
                                    if (referralResult is OkObjectResult referralOkResult)
                                    {
                                        Console.WriteLine("Referral code created");
                                        await _signInManager.SignInAsync(user, isPersistent: false);
                                        Console.WriteLine("User logged in");
                                        return new OkObjectResult(new { Message = "Business added successfully", Token = token, ReferralId = referralOkResult.Value });
                                    }
                                    return referralResult;
                                }
                                else if (referralType.ToString() == "B")
                                {
                                    var referrerBusinessId = await _referralRepo.GetFkReferredBusinessId(business.FkReferralId);
                                    ReferralDTO referralDTO = new ReferralDTO
                                    {
                                        FkReferrerBusinessId = referrerBusinessId.ToString(),
                                        FkReferredBusinessId = business.PkBusinessId,
                                    };

                                    var referralResult = await _referralRepo.CreateReferralCodeForBusiness(referralDTO);
                                    if (referralResult is OkObjectResult referralOkResult)
                                    {
                                        Console.WriteLine("Referral code created");
                                        await _signInManager.SignInAsync(user, isPersistent: false);
                                        Console.WriteLine("User logged in");
                                        return new OkObjectResult(new { Message = "Business added successfully", Token = token, ReferralId = referralOkResult.Value });
                                    }
                                    return referralResult;
                                }
                            }
                            else
                            {
                                ReferralDTO referralDTO = new ReferralDTO
                                {
                                    FkReferredBusinessId = business.PkBusinessId
                                };
                                var referralResult = await _referralRepo.CreateReferralCodeForBusiness(referralDTO);
                                if (referralResult is OkObjectResult referralOkResult)
                                {
                                    Console.WriteLine("Referral code created");
                                    await _signInManager.SignInAsync(user, isPersistent: false);
                                    Console.WriteLine("User logged in");
                                    return new OkObjectResult(new { Message = "Business added successfully", Token = token, ReferralId = referralOkResult.Value });
                                }
                                return referralResult;
                            }
                        }
                        return new BadRequestObjectResult(new { Errors = addUserRoleResult.Errors });
                    }
                    return new BadRequestObjectResult(new { Errors = addUserResult.Errors });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error adding customer: {ex.Message}");
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
    }
}
