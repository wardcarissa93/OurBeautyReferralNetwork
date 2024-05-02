using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Models;
using Stripe;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,  // Amount should be in the smallest currency unit
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
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
