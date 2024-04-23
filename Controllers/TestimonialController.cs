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
    public class TestimonialController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public TestimonialController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/testimonial/{testimonialId}")]

        public virtual IActionResult GetFee([FromRoute][Required] int testimonialId)
        {
            TestimonialRepo testimonialRepo = new TestimonialRepo(_context, _obrnContext);

            var testimonial = testimonialRepo.GetTestimonialById(testimonialId);
            if (testimonial == null)
            {
                return NotFound(); // Return a 404 Not Found response if testimonialId does not exist
            }
            return Ok(testimonial);
        }

        [HttpGet]
        [Route("/testimonial")]
        //[ValidateModelState]
        [SwaggerOperation("TestimonialGet")]
        public virtual IActionResult TestimonialGet()
        {
            TestimonialRepo testimonialRepo = new TestimonialRepo(_context, _obrnContext);
            var testimonials = testimonialRepo.GetAllTestimonials();
            return Ok(testimonials);
        }
    }
}
