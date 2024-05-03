using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Models;
using Stripe;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]/{UserId}")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request, string UserId)
        {
            var paymentIntentService = new PaymentIntentService();
            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,  // Amount should be in the smallest currency unit
                Currency = "cad",
                PaymentMethodTypes = new List<string> { "card" },
                ReturnUrl = "https://calm-hill-024d52d1e.5.azurestaticapps.net/" + $"CheckOut/OrderConfirmation",
                Customer = UserId,


            };

            try
            {
                var paymentIntent = await paymentIntentService.CreateAsync(createOptions);
                return Ok(new { PaymentIntentId = paymentIntent.Id });
            }
            catch (StripeException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
