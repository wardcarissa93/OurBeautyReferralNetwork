using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using Stripe;
using Stripe.Checkout;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly CustomerRepo _customerRepo;
        private readonly BusinessRepo _businessRepo;

        public PaymentController(IConfiguration configuration, CustomerRepo customerRepo, BusinessRepo businessRepo)
        {
            _configuration = configuration;
            _customerRepo = customerRepo;
            _businessRepo = businessRepo;
            _customerRepo = customerRepo;
        }



        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession(string userId, string itemId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey"] ?? throw new Exception("StripeKey not found in configuration");



                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            Price = itemId, // Replace with your actual price ID or amount
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    CustomerCreation = "always",
                    ClientReferenceId = userId,
                    SuccessUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/CheckOut/OrderConfirmation",
                    CancelUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/",
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                // Redirect the customer to the Stripe-hosted checkout page

                return Ok(new { url = session.Url, sessionId = session.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("create-checkout-session-subscription")]
        public async Task<IActionResult> CreateCheckoutSessionSubscription()
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeKey"] ?? throw new Exception("StripeKey not found in configuration");

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            Price = "price_1PDHjEEq0H2sm5gKUcXpb9fv",
                            Quantity = 1,
                        },

                    },
                    Mode = "subscription",
                    SuccessUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/CheckOut/OrderConfirmation",
                    CancelUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/",
                };
                var service = new SessionService();
                var session = await service.CreateAsync(options);

                // Redirect the customer to the Stripe-hosted checkout page using the hosted checkout link
                var hostedCheckoutLink = $"https://buy.stripe.com/test_7sIbJb1C9eMzh20eUU";
                return Ok(hostedCheckoutLink);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpPost("webhook")]

        public async Task<IActionResult> WebhookHandler()
        {
            var webhookSecret = _configuration["Webhook:Secret"];

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                Event? stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], webhookSecret);

                var session = stripeEvent.Data.Object as Session;

                var userId = session.ClientReferenceId;

                var customer = await _customerRepo.GetCustomerById(userId);
                var business = await _businessRepo.GetBusinessById(userId);

                if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    Charge? charge = stripeEvent.Data.Object as Charge;
                    if (charge != null)
                    {
                        TransactionRepo transactionRepo = new TransactionRepo(_context, _obrnContext);
                        try
                        {
                            if (customer != null)
                            {
                                Transaction transaction = transactionRepo.CreateTransactionForCustomer(charge, userId);
                                // Log successful transaction for customer
                                Console.WriteLine($"Successful charge for customer {userId}: {charge.Amount}");
                            }
                            else if (business != null)
                            {
                                Transaction transaction = transactionRepo.CreateTransactionForBusiness(charge, userId);
                                // Log successful transaction for business
                                Console.WriteLine($"Successful charge for business {userId}: {charge.Amount}");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log transaction creation error
                            Console.WriteLine($"Error creating transaction: {ex.Message}");
                            return StatusCode(500); // Internal Server Error
                        }
                    }

                    return Ok();
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    return Ok(); // or BadRequest() depending on your handling logic
                }
            }
            catch (StripeException e)
            {
                Console.WriteLine("Stripe Error: {0}", e.Message);
                return BadRequest(); // or StatusCode(500) if it's a critical error
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return StatusCode(500); // Internal Server Error
            }
        }
    }
}


//[HttpPost("cancel-subscription")]
//public async Task<IActionResult> CancelSubscription(string userId)
//{
//    // Retrieve session ID associated with the user (from your database)
//    var sessionId = GetSessionIdForUser(userId); // Example method to get sessionId

//    // Cancel the subscription session using the Stripe API
//    var service = new SessionService();
//    var canceledSession = await service.CancelAsync(sessionId);

//    // Handle cancellation result (e.g., update database status)
//    if (canceledSession.Status == "canceled")
//    {
//        // Update user's subscription status in your database
//        return Ok("Subscription canceled successfully");
//    }
//    else
//    {
//        return BadRequest("Failed to cancel subscription");
//    }
//}

