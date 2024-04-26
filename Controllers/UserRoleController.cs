using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Repositories;

namespace OurBeautyReferralNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly UserRoleRepo _userRoleRepo;

        public UserRoleController(UserRoleRepo userRoleRepo)
        {
            _userRoleRepo = userRoleRepo;
        }

        [HttpPost("adduserrole")]
        public async Task<IActionResult> AddUserRole(string email, string roleName)
        {
            var result = await _userRoleRepo.AddUserRoleAsync(email, roleName);
            return result;
        }
    }
}
