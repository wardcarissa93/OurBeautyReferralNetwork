using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Utilities;
using System;
using System.Threading.Tasks;

namespace OurBeautyReferralNetwork.Repositories
{
    public class CustomerRepo
    {
        private readonly JWTUtilities _jWTUtilities;
        private readonly obrnDbContext _obrnDbContext;

        public CustomerRepo(JWTUtilities jWTUtilities,
                            obrnDbContext obrnDbContext)
        {
            this._jWTUtilities = jWTUtilities;
            _obrnDbContext = obrnDbContext;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            IEnumerable<Customer> customers = _obrnDbContext.Customers.ToList();
            return customers;
        }

        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            try
            {
                _obrnDbContext.Customers.Add(customer);
                await _obrnDbContext.SaveChangesAsync();

                // Generate JWT for the added customer
                var token = _jWTUtilities.GenerateJwtToken(customer.Email);

                return new OkObjectResult(new { Message = "Customer added successfully", Token = token });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error adding customer: {ex.Message}");
            }
        }
    }
}