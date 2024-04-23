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
    public class FeeController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public FeeController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/fee/{feeId}")]

        public virtual IActionResult GetFee([FromRoute][Required] string feeId)
        {
            FeeRepo feeRepo = new FeeRepo(_context, _obrnContext);
            var fee = feeRepo.GetFeeById(feeId);
            if (fee != null)
            {
                return Ok(fee);

            }
            return NotFound("Fee not found");
        }

        [HttpGet]
        [Route("/fee")]
        //[ValidateModelState]
        [SwaggerOperation("FeeGet")]
        public virtual IActionResult FeeGet()
        {
            FeeRepo feeRepo = new FeeRepo(_context, _obrnContext);
            var fees = feeRepo.GetAllFees();
            return Ok(fees);
        }

        [HttpPost]
        [Route("/fee/create")]
        public IActionResult Create(FeeAndCommission fee)
        {
            FeeRepo feeRepo = new FeeRepo(_context, _obrnContext);
            bool isSuccess = feeRepo.CreateFee(fee);

            if (isSuccess)
            {
                return CreatedAtAction(nameof(GetFee), new { feeId = fee.PkFeeId }, fee);
            }
            return BadRequest();
            
        }

        [HttpPut("{feeId}")]
        public IActionResult Update(string feeId, FeeAndCommission fee)
        {
            if (feeId != fee.PkFeeId)
                return BadRequest();

            FeeRepo feeRepo = new FeeRepo(_context, _obrnContext);
            var existingFee = feeRepo.GetFeeById(feeId);
            if (existingFee is null)
            {
                return NotFound("Fee not found with the provided ID.");

            }
            return NoContent();
        }

        [HttpDelete("{feeId}")]
        [SwaggerOperation("Delete")]
        public IActionResult Delete(string feeId)
        {
            FeeRepo feeRepo = new FeeRepo(_context, _obrnContext);
            string message = feeRepo.Delete(feeId);
            if (message == "Fee does not exist")
            {
                return NotFound();
            }
            else if (message == "Deleted successfully")
            {
                return Ok(); // server successfully processed the request and there is no content to send in the response payload.
            }
            else
            {
                // Handle other potential error cases, such as database errors
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
