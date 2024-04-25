using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Utilities;

namespace OurBeautyReferralNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTestController : ControllerBase
    {
        private readonly JWTUtilities _jwtUtilities;

        public JwtTestController(JWTUtilities jwtUtilities)
        {
            _jwtUtilities = jwtUtilities;
        }

        [HttpGet("GenerateRandomKey/{keyLength}")]
        [AllowAnonymous]
        public IActionResult GenerateRandomKey(int keyLength)
        {
            var randomKey = JWTUtilities.GenerateRandomKey(keyLength);
            return Ok(new { RandomKey = randomKey });
        }

        [HttpGet("GenerateJwtToken")]
        [AllowAnonymous]
        public IActionResult GenerateJwtToken(string email)
        {
            var token = _jwtUtilities.GenerateJwtToken(email);
            return Ok(new { Token = token });
        }
    }
}
