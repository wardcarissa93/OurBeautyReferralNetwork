using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

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
                    SuccessUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/CheckOut/OrderConfirmation",
                    CancelUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/",
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                // Redirect the customer to the Stripe-hosted checkout page

                return Ok(new {url = session.Url});
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

    }
}
