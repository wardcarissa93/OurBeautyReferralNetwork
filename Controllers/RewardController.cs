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
    public class RewardController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public RewardController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/reward/{rewardId}")]

        public virtual IActionResult GetFee([FromRoute][Required] int rewardId)
        {
            RewardRepo rewardRepo = new RewardRepo(_context, _obrnContext);

            var reward = rewardRepo.GetRewardById(rewardId);
            if (reward == null)
            {
                return NotFound(); // Return a 404 Not Found response if testimonialId does not exist
            }
            return Ok(reward);
        }

        [HttpGet]
        [Route("/reward")]
        //[ValidateModelState]
        [SwaggerOperation("RewardGet")]
        public virtual IActionResult RewardGet()
        {
            RewardRepo rewardRepo = new RewardRepo(_context, _obrnContext);
            var rewards = rewardRepo.GetAllRewards();
            return Ok(rewards);
        }
    }
}
