using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.BusinessModels;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;

namespace OurBeautyReferralNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly BusinessRepo _businessRepo;

        public BusinessController(BusinessRepo businessRepo)
        {
            _businessRepo = businessRepo;
        }

        [HttpGet("getbusinesses")]
        public ActionResult<IEnumerable<Business>> GetBusinesses()
        {
            var businesses = _businessRepo.GetAllBusinesses();
            return Ok(businesses);
        }

        [HttpGet("getbusiness/{id}")]
        public async Task<IActionResult> GetBusinessById(string id)
        {
            var result = await _businessRepo.GetBusinessById(id);
            return result;
        }

        [HttpGet("getbusinessbyemail")]
        public async Task<IActionResult> GetBusinessByEmail(string email)
        {
            var result = await _businessRepo.GetBusinessByEmail(email);
            return result;
        }

        [HttpPost("addbusiness")]
        public async Task<IActionResult> AddBusiness(RegisterBusiness model)
        {
            var result = await _businessRepo.AddBusiness(model);
            return result;
        }

        [HttpPost("editbusiness")]
        public async Task<IActionResult> EditBusiness(EditBusiness business)
        {
            var result = await _businessRepo.EditBusiness(business);
            return result;
        }

        [HttpDelete("deletebusiness/{id}")]
        public async Task<IActionResult> DeleteBusiness(string id)
        {
            var result = await _businessRepo.DeleteBusiness(id);
            return result;
        }
    }
}