using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OurBeautyReferralNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public UserController(ApplicationDbContext context,obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _context = context;
            _obrnContext = obrnContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("getusers")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _obrnContext.Feeandcommissions.ToList();
            return Ok(users);
        }

        [HttpPost("adduser")]
        public async Task<IActionResult> AddUser(User model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { Message = "User added successfully" });
            }

            return BadRequest(result.Errors);
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Issuer"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
