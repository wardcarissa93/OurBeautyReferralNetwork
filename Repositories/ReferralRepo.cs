using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using System.Drawing;
using System.Text;

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

        public async Task<string> GetReferralTypeById(string referralId)
        {
            try
            {
                obrnDbContext newObrnDbContext = new obrnDbContext();
                var referral = await newObrnDbContext.Referrals.FirstOrDefaultAsync(r => r.PkReferralId == referralId);
                if (referral != null)
                {
                    return referral.ReferredType;
                }
                else
                {
                    throw new KeyNotFoundException("Referral not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting referral type: {ex.Message}");
            }
        }

        public async Task<string> GetCustomerReferralCode(string customerId)
        {
            try
            {
                obrnDbContext newObrnDbContext = new obrnDbContext();
                var referral = await newObrnDbContext.Referrals.FirstOrDefaultAsync(r => r.FkReferredCustomerId == customerId);
                if (referral != null)
                {
                    return referral.PkReferralId;
                }
                else
                {
                    throw new KeyNotFoundException("Referral not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting referral code: {ex.Message}");
            }
        }

        public async Task<string> GetFkReferredCustomerId(string referralId)
        {
            try
            {
                obrnDbContext newObrnDbContext = new obrnDbContext();
                var referral = await newObrnDbContext.Referrals.FirstOrDefaultAsync(r => r.PkReferralId == referralId);
                if (referral != null)
                {
                    return referral.FkReferredCustomerId;
                }
                else
                {
                    throw new KeyNotFoundException("Referral not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting fkReferredCustomerId: {ex.Message}");
            }
        }

        public async Task<string> GetFkReferredBusinessId(string referralId)
        {
            try
            {
                obrnDbContext newObrnDbContext = new obrnDbContext();
                var referral = await newObrnDbContext.Referrals.FirstOrDefaultAsync(r => r.PkReferralId == referralId);
                if (referral != null)
                {
                    return referral.FkReferredBusinessId;
                }
                else
                {
                    throw new KeyNotFoundException("Referral not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting fkReferredBusinessId: {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateReferralCodeForCustomer(ReferralDTO referralDTO)
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
                    FkReferredCustomerId = referralDTO.FkReferredCustomerId ?? null,
                    FkReferrerCustomerId = referralDTO.FkReferrerCustomerId ?? null,
                    FkReferrerBusinessId = referralDTO.FkReferrerBusinessId ?? null,
                    FkReferredBusinessId = referralDTO.FkReferredBusinessId ?? null,
                    ReferralDate = DateOnly.FromDateTime(DateTime.Today),
                    Status = "",
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

        public async Task<IActionResult> CreateReferralCodeForBusiness(ReferralDTO referralDTO)
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
                    FkReferredCustomerId = referralDTO.FkReferredCustomerId ?? null,
                    FkReferrerCustomerId = referralDTO.FkReferrerCustomerId ?? null,
                    FkReferredBusinessId = referralDTO.FkReferredBusinessId ?? null,
                    FkReferrerBusinessId = referralDTO.FkReferrerBusinessId ?? null,
                    ReferralDate = DateOnly.FromDateTime(DateTime.Today),
                    Status = "",
                    ReferredType = "B"
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

        //public async Task<IActionResult> EditReferral(ReferralDTO referralDTO)
        //{
        //    try
        //    {
        //        // Check if the referral code exists in the database
        //        var existingReferral = await _obrnContext.Referrals.FirstOrDefaultAsync(r => r.PkReferralId == referralDTO.PkReferralId);
        //        if (existingReferral == null)
        //        {
        //            return new NotFoundObjectResult("Referral code not found");
        //        }

        //        // Update the referral's properties
        //        existingReferral.FkReferrerCustomerId = referralDTO.FkReferrerCustomerId;
        //        existingReferral.FkReferredCustomerId = referralDTO.FkReferredCustomerId;
        //        existingReferral.FkReferrerBusinessId = referralDTO.FkReferrerBusinessId;
        //        existingReferral.FkReferredBusinessId = referralDTO.FkReferredBusinessId;
        //        existingReferral.ReferralDate = DateOnly.FromDateTime(DateTime.Today);
        //        existingReferral.Status = "fulfilled";

        //        // Save changes to the database
        //        await _obrnContext.SaveChangesAsync();

        //        return new OkObjectResult("Referral completed successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BadRequestObjectResult($"Error completing referral: {ex.Message}");
        //    }
        //}
    }
}
