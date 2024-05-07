using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
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
                            Price = "price_1PDEdTEq0H2sm5gKFzTAU6L5", // Replace with your actual price ID or amount
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    CustomerCreation = "always",
                    SuccessUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/CheckOut/success={CHECKOUT_SESSION_ID}",
                    CancelUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/",
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                // Redirect the customer to the Stripe-hosted checkout page

                return Ok(new {url = session.Url, sessionId = session.Id});
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }



        [HttpPost]
        [Route("CheckOut/success={session_id}")]

        public IActionResult CreateTransaction([FromRoute][Required] string session_id, decimal tax)
        {
            TransactionRepo transactionRepo = new TransactionRepo(_context, _obrnContext);
            Transaction createdTransaction = transactionRepo.CreateTransactionForBusiness(session_id, tax);

            if (createdTransaction != null)
            {
                return CreatedAtAction(nameof(TransactionGetAll), createdTransaction);
            }
            return BadRequest("Failed to create the transaction");

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
        [HttpGet]
        [Route("/transactions")]
        //[ValidateModelState]
        [SwaggerOperation("TransactionGetAll")]
        public virtual IActionResult TransactionGetAll()
        {
            TransactionRepo transactionRepo = new TransactionRepo(_context, _obrnContext);
            var transactions = transactionRepo.GetAllTransactionsBase();
            return Ok(transactions);
        }



    }
}
