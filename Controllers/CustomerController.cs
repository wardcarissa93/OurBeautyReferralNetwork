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
        private readonly JWTUtilities _jwtUtilities;

        public CustomerController(ApplicationDbContext context,
                                  IConfiguration configuration,
                                  CustomerRepo customerRepo,
                                  JWTUtilities jWTUtilities)
        {
            _context = context;
            _configuration = configuration;
            _customerRepo = customerRepo;
            _jwtUtilities = jWTUtilities;
        }

        [HttpGet("getcustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _customerRepo.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPost("addcustomer")]
        public async Task<IActionResult> AddCustomer(Customer model)
        {
            var result = await _customerRepo.AddCustomer(model);
            return result;
        }
    }
}