using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class ReferralRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public ReferralRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Referral> GetAllReferrals()
        {
            IEnumerable<Referral> referrals = _obrnContext.Referrals.ToList();
            return referrals;
        }
        public async Task<IActionResult> GetReferralById(string referralId)
        {
            try
            {
                var referral = await _obrnContext.Referrals.FirstOrDefaultAsync(r => r.PkReferralId == referralId);
                if (referral != null)
                {
                    return new OkObjectResult(referral);
                }
                else
                {
                    return new NotFoundObjectResult("Referral not found.");
                }
            }
            catch (Exception ex) 
            { 
                return new BadRequestObjectResult($"Error getting referral: {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateReferralCodeForCustomer(string customerId)
        {
            try
            {
                // Generate a random 8-character alphanumeric string
                string referralId;
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
                do
                {
                    referralId = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
                } while (_obrnContext.Referrals.Any(r => r.PkReferralId == referralId));

                // Create a new Referral object
                Referral newReferral = new Referral
                {
                    PkReferralId = referralId,
                    FkReferrerCustomerId = customerId,
                    ReferralDate = DateOnly.FromDateTime(DateTime.Today),
                    Status = "pending",
                    ReferredType = "C"
                };

                // Add and save the new Referral
                _obrnContext.Referrals.Add(newReferral);
                await _obrnContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Referral created successfully", ReferralId = referralId }); ;
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error creating referral: {ex.Message}");
            }
        }
    }
}
