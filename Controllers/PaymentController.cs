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
using System.Runtime.InteropServices;
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


        public PaymentController(IConfiguration configuration, CustomerRepo customerRepo, obrnDbContext obrnDbContext, BusinessRepo businessRepo)
        {
            _configuration = configuration;
            _customerRepo = customerRepo;
            _businessRepo = businessRepo;
            _customerRepo = customerRepo;
            _obrnContext = obrnDbContext;
        }



        [HttpPost("create-checkout-session/{userId}/{itemId}")]

        public async Task<IActionResult> CreateCheckoutSession(string userId, string itemId)
        {
            try
            {
                // Ensure Stripe API Key is set properly
                if (string.IsNullOrEmpty(StripeConfiguration.ApiKey) || StripeConfiguration.ApiKey == "SKey not found")
                {
                    throw new Exception("StripeKey not found in configuration");
                }

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
                    SuccessUrl = "http://localhost:5173/CheckOut/OrderConfirmation",
                    CancelUrl = "http://localhost:5173/",

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

        [HttpPost("create-checkout-session-subscription/{userId}/{itemId}")]
        public async Task<IActionResult> CreateCheckoutSessionSubscription(string userId, string itemId)
        {
            try
            {
                var webhookSecret = _configuration["Webhook:Secret"];

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            Price = itemId,
                            Quantity = 1,
                        },

                    },
                    Mode = "subscription",
                    ClientReferenceId = userId,
                    SuccessUrl = "http://localhost:5173/CheckOut/OrderConfirmation",
                    CancelUrl = "http://localhost:5173/",
                    Metadata = new Dictionary<string, string> { { "subscription_stripe_id", itemId } },
                };
                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return Ok(new { url = session.Url, sessionId = session.Id });
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
            var userId = "";

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                Event? stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], webhookSecret);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    Session? session = stripeEvent.Data.Object as Session;

                    Console.WriteLine("Charge: ", session?.AmountTotal);
                    Console.WriteLine("User Id: ", session?.ClientReferenceId);

                    userId = session?.ClientReferenceId;
                    TransactionRepo transactionRepo = new TransactionRepo(_context, _obrnContext);
                    var customer = await _customerRepo.GetCustomerById(userId);
                    var business = await _businessRepo.GetBusinessById(userId);
                    try
                    {
                        if (customer is OkObjectResult)
                        {
                            Transaction transaction = transactionRepo.CreateTransactionForCustomer(session, userId);
                            // Log successful transaction for customer
                            Console.WriteLine($"Successful charge for customer {userId}: {session.AmountTotal}");
                        }
                        else if (business is OkObjectResult)
                        {
                            Transaction transaction = transactionRepo.CreateTransactionForBusiness(session, userId);
                            if (session.Mode == "subscription")
                            {
                                String stripeId = session?.Metadata["subscription_stripe_id"];

                                var transactionId = transaction.PkTransactionId;
                                FeeRepo feeRepo = new FeeRepo(_context, _obrnContext);
                                
                                SubscriptionRepo subscriptionRepo = new SubscriptionRepo(_context, _obrnContext, feeRepo);
                                OurBeautyReferralNetwork.Models.Subscription subscription = subscriptionRepo.CreateSubscriptionForBusiness(session, userId, stripeId, transactionId);
                            }
                            // Log successful transaction for business
                            Console.WriteLine($"Successful charge for business {userId}: {session.AmountTotal}");
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

        //[HttpPost("cancel-subscription/{subscriptionId}")]
        //public async Task<IActionResult> CancelSubscription(string subscriptionId)
        //{
        //    // Cancel the subscription session using the Stripe API
        //    var service = new SubscriptionService();
        //    var canceledSession = service.CancelAsync(subscriptionId);

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
    }
}




