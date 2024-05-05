using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;

namespace OurBeautyReferralNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralController : ControllerBase
    {
        private readonly ReferralRepo _referralRepo;
        private readonly obrnDbContext _obrnDbContext;

        public ReferralController(ReferralRepo referralRepo,
                                  obrnDbContext obrnDbContext)
        {
            _referralRepo = referralRepo;
            _obrnDbContext = obrnDbContext;
        }

        [HttpGet("get-referrals")]
        public ActionResult<IEnumerable<Referral>> GetReferrals()
        {
            var referrals = _referralRepo.GetAllReferrals();
            return Ok(referrals);
        }

        [HttpGet("get-referral/{id}")]
        public async Task<IActionResult> GetReferralById(string id)
        {
            var result = await _referralRepo.GetReferralById(id);
            return result;
        }

        [HttpGet("get-referral-type/{id}")]
        public async Task<string> GetReferralTypeById(string id)
        {
            var result = await _referralRepo.GetReferralTypeById(id);
            return result;
        }

        [HttpPost("create-referral-customer")]
        public async Task<IActionResult> CreateReferralCodeForCustomer(ReferralDTO referralDTO)
        {
            var result = await _referralRepo.CreateReferralCodeForCustomer(referralDTO);
            return result;
        }

        [HttpPost("create-referral-business")]
        public async Task<IActionResult> CreateReferralCodeForBusiness(ReferralDTO referralDTO)
        {
            var result = await _referralRepo.CreateReferralCodeForBusiness(referralDTO);
            return result;
        }

        //[HttpPost("edit-referral")]
        //public async Task<IActionResult> EditReferral(ReferralDTO referralDTO)
        //{
        //    var result = await _referralRepo.EditReferral(referralDTO);
        //    return result;
        //}
    }
}
