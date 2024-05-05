using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.DataTransferObjects;
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

        [HttpGet("get-businesses")]
        public ActionResult<IEnumerable<Business>> GetBusinesses()
        {
            var businesses = _businessRepo.GetAllBusinesses();
            return Ok(businesses);
        }

        [HttpGet("get-business/{id}")]
        public async Task<IActionResult> GetBusinessById(string id)
        {
            var result = await _businessRepo.GetBusinessById(id);
            return result;
        }

        [HttpGet("get-business-by-email")]
        public async Task<IActionResult> GetBusinessByEmail(string email)
        {
            var result = await _businessRepo.GetBusinessByEmail(email);
            return result;
        }

        [HttpPost("add-business")]
        public async Task<IActionResult> AddBusiness(RegisterBusinessDTO model)
        {
            var result = await _businessRepo.AddBusiness(model);
            return result;
        }

        [HttpPost("edit-business")]
        public async Task<IActionResult> EditBusiness(EditBusinessDTO business)
        {
            var result = await _businessRepo.EditBusiness(business);
            return result;
        }

        [HttpDelete("delete-business/{id}")]
        public async Task<IActionResult> DeleteBusiness(string id)
        {
            var result = await _businessRepo.DeleteBusiness(id);
            return result;
        }
    }
}