using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OurBeautyReferralNetwork.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public DiscountController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/discount/{discountId}")]

        public virtual IActionResult GetDiscountById([FromRoute][Required] string discountId)
        {
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            var discount = discountRepo.GetDiscountById(discountId);
            if (discount == null)
            {
                return NotFound(); // Return a 404 Not Found response if discountId does not exist
            }
            return Ok(discount);
        }

        [HttpGet]
        [Route("/discount")]
        //[ValidateModelState]
        [SwaggerOperation("DiscountGet")]
        public virtual IActionResult DiscountGet()
        {
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            var discounts = discountRepo.GetAllDiscounts();
            return Ok(discounts);
        }

        [HttpPost]
        [Route("/discount/create")]
        public IActionResult Create(Discount discount)
        {
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            bool isSuccess = discountRepo.CreateDiscount(discount);

            if (isSuccess)
            {
                return CreatedAtAction(nameof(DiscountGet), new { discountId = discount.PkDiscountId }, discount);
            }
            return BadRequest();

        }

        [HttpDelete("{discountId}")]
        [SwaggerOperation("Delete")]
        public IActionResult Delete(string discountId)
        {
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            string message = discountRepo.Delete(discountId);
            if (message == "Discount does not exist")
            {
                return NotFound();
            }
            else if (message == "Deleted successfully")
            {
                return Ok(); // server successfully processed the request and there is no content to send in the response payload.
            }
            else if (message == "this discount is currently in use")
            {
                return Conflict("Discount is currently in use and cannot be deleted");
            }
            else
            {
                // Handle other potential error cases, such as database errors
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
