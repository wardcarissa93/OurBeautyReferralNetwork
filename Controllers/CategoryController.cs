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
    public class CatetoryController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public CatetoryController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/category/{categoryId}")]

        public virtual IActionResult GetCategoryById([FromRoute][Required] int categoryId)
        {
            CategoryRepo categoryRepo = new CategoryRepo(_context, _obrnContext);
            var category = categoryRepo.GetCategoryById(categoryId);
            if (category == null)
            {
                return NotFound(); // Return a 404 Not Found response if discountId does not exist
            }
            return Ok(category);
        }

        [HttpGet]
        [Route("/category")]
        //[ValidateModelState]
        [SwaggerOperation("CategoryGet")]
        public virtual IActionResult CategoryGet()
        {
            CategoryRepo categoryRepo = new CategoryRepo(_context, _obrnContext);
            var categories = categoryRepo.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost]
        [Route("/category/create")]
        public IActionResult Create(Category category)
        {
            CategoryRepo categoryRepo = new CategoryRepo(_context, _obrnContext);
            bool isSuccess = categoryRepo.CreateCategory(category);

            if (isSuccess)
            {
                return CreatedAtAction(nameof(CategoryGet), new { categoryId = category.PkCategoryId }, category);
            }
            return BadRequest();

        }


        [HttpDelete("{categoryId}")]
        [SwaggerOperation("Delete")]
        public IActionResult Delete(int categoryId)
        {
            CategoryRepo categoryRepo = new CategoryRepo(_context, _obrnContext);
            string message = categoryRepo.Delete(categoryId);
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
