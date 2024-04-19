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
        private readonly ApplicationDbContext _context;
        private readonly JWTUtilities _jWTUtilities;

        public CustomerRepo(ApplicationDbContext context,
                            JWTUtilities jWTUtilities)
        {
            this._context = context;
            this._jWTUtilities = jWTUtilities;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            IEnumerable<Customer> customers = _context.Customers.ToList();
            return customers;
        }

        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

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