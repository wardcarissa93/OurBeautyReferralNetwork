using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using OurBeautyReferralNetwork.Utilities;
using System;
using System.Threading.Tasks;

namespace OurBeautyReferralNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly CustomerRepo _customerRepo;

        public CustomerController(ApplicationDbContext context,
                                  IConfiguration configuration,
                                  CustomerRepo customerRepo)
        {
            _context = context;
            _configuration = configuration;
            _customerRepo = customerRepo;
        }

        [HttpGet("getcustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _customerRepo.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPost("addcustomer")]
        public async Task<IActionResult> AddCustomer(RegisterCustomer model)
        {
            var result = await _customerRepo.AddCustomer(model);
            return result;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User model)
        {
            var result = await _customerRepo.Login(model);
            return result;
        }
    }
}