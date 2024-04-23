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
    public class ReviewController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public ReviewController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/review/{reviewId}")]

        public virtual IActionResult GetReviewById([FromRoute][Required] int reviewId)
        {
            ReviewRepo reviewRepo = new ReviewRepo(_context, _obrnContext);
            var review = reviewRepo.GetReviewById(reviewId);
            if (review == null)
            {
                return NotFound(); // Return a 404 Not Found response if discountId does not exist
            }
            return Ok(review);
        }

        [HttpGet]
        [Route("/review")]
        //[ValidateModelState]
        [SwaggerOperation("ReviewGet")]
        public virtual IActionResult ReviewGet()
        {
            ReviewRepo reviewRepo = new ReviewRepo(_context, _obrnContext);
            var reviews = reviewRepo.GetAllReviews();
            return Ok(reviews);
        }
    }
}
