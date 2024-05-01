using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurBeautyReferralNetwork.CustomerModels;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Utilities;


namespace OurBeautyReferralNetwork.Repositories
{
    public class CustomerRepo
    {
        private readonly JWTUtilities _jWTUtilities;
        private readonly obrnDbContext _obrnDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ReferralRepo _referralRepo;

        public CustomerRepo(JWTUtilities jWTUtilities,
                            obrnDbContext obrnDbContext,
                            UserManager<IdentityUser> userManager,
                            ReferralRepo referralRepo)
        {
            _jWTUtilities = jWTUtilities;
            _obrnDbContext = obrnDbContext;
            _userManager = userManager;
            _referralRepo = referralRepo;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            IEnumerable<Customer> customers = _obrnDbContext.Customers.ToList();
            return customers;
        }

        public async Task<IActionResult> GetCustomerById(string customerId)
        {
            try
            {
                var customer = await _obrnDbContext.Customers.FirstOrDefaultAsync(c => c.PkCustomerId == customerId);
                if (customer != null)
                {
                    return new OkObjectResult(customer);
                }
                else
                {
                    return new NotFoundObjectResult("Customer not found");
                }
            }
            catch (Exception ex) 
            {
                return new BadRequestObjectResult($"Error getting customer: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            try
            {
                var customer = await _obrnDbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
                if (customer != null)
                {
                    return new OkObjectResult(customer);
                }
                else
                {
                    return new NotFoundObjectResult("Customer not found");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error getting customer: {ex.Message}");
            }
        }

        public async Task<IActionResult> AddCustomer(RegisterCustomer customer)
        {
            try
            {
                // Check if a customer with the same PkCustomerId already exists
                var existingCustomer = await _obrnDbContext.Customers.FirstOrDefaultAsync(c => c.PkCustomerId == customer.PkCustomerId);
                if (existingCustomer != null)
                {
                    return new BadRequestObjectResult("Username unavailable. Please enter a different username.");
                }

                Customer newCustomer = new Customer
                {
                    PkCustomerId = customer.PkCustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    Birthdate = customer.Birthdate,
                    Email = customer.Email,
                    Vip = customer.Vip,
                    Confirm18 = customer.Confirm18
                };

                _obrnDbContext.Customers.Add(newCustomer);
                await _obrnDbContext.SaveChangesAsync();

                var user = new IdentityUser
                {
                    UserName = customer.PkCustomerId,
                    Email = customer.Email
                };

                var addUserResult = await _userManager.CreateAsync(user, customer.Password);

                if (addUserResult.Succeeded)
                {
                    var addUserRoleResult = await _userManager.AddToRoleAsync(user, "customer");

                    if (addUserRoleResult.Succeeded)
                    {
                        // Generate JWT for the added customer
                        var token = _jWTUtilities.GenerateJwtToken(customer.Email);

                        // Create a referral code for the customer
                        var referralResult = await _referralRepo.CreateReferralCodeForCustomer(customer.PkCustomerId);
                        if (referralResult is OkObjectResult referralOkResult)
                        {
                            return new OkObjectResult(new { Message = "Customer added successfully", Token = token, ReferralId = referralOkResult.Value });
                        }

                        return referralResult;
                    }

                    return new BadRequestObjectResult(new { Errors = addUserRoleResult.Errors });
                }

                return new BadRequestObjectResult(new { Errors = addUserResult.Errors });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error adding customer: {ex.Message}");
            }
        }
        public async Task<IActionResult> Login(User model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Login successful, generate JWT token
                var token = _jWTUtilities.GenerateJwtToken(user.Email);

                return new OkObjectResult(new { Message = "Login successful", Token = token });
            }

            return new BadRequestObjectResult(new { Message = "Invalid email or password" });
        }

        public async Task<IActionResult> EditCustomer(EditCustomer customer)
        {
            try
            {
                // Check if the customer exists in the database
                var existingCustomer = await _obrnDbContext.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);
                if (existingCustomer == null)
                {
                    return new NotFoundObjectResult("Customer not found");
                }

                // Update the customer's properties
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Address = customer.Address;
                existingCustomer.City = customer.City;
                existingCustomer.Province = customer.Province;
                existingCustomer.PostalCode = customer.PostalCode;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Birthdate = customer.Birthdate;
                existingCustomer.Email = customer.Email;
                existingCustomer.Vip = customer.Vip;

                // Save changes to the database
                await _obrnDbContext.SaveChangesAsync();

                return new OkObjectResult("Customer updated successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error editing customer: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdatePassword(EditPassword password)
        {
            try
            {
                // Find the user by ID
                var user = await _userManager.FindByIdAsync(password.UserId);
                if (user == null)
                {
                    return new NotFoundObjectResult("User not found");
                }

                // Check if the current password matches
                var passwordCheck = await _userManager.CheckPasswordAsync(user, password.CurrentPassword);
                if (!passwordCheck)
                {
                    return new BadRequestObjectResult("Incorrect current password");
                }

                // Check if the new password matches the confirmation password
                if (password.NewPassword != password.ConfirmPassword)
                {
                    return new BadRequestObjectResult("New password and confirmation password do not match");
                }

                // Update the user's password
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, password.NewPassword);

                if (result.Succeeded)
                {
                    return new OkObjectResult("Password updated successfully");
                }
                return new BadRequestObjectResult("Error updating password");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error updating password: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
            try
            {
                // Find the customer by ID
                var customer = await _obrnDbContext.Customers.FirstOrDefaultAsync(c => c.PkCustomerId == customerId);
                if (customer == null)
                {
                    return new NotFoundObjectResult("Customer not found");
                }

                // Find the corresponding AspNetUser by email
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user == null)
                {
                    return new NotFoundObjectResult("User not found");
                }

                // Delete the customer
                _obrnDbContext.Customers.Remove(customer);
                await _obrnDbContext.SaveChangesAsync();

                // Delete the AspNetUser
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Handle delete user error if needed
                    return new BadRequestObjectResult("Error deleting user");
                }

                return new OkObjectResult("Customer and associated user deleted successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error deleting customer: {ex.Message}");
            }
        }
    }
}