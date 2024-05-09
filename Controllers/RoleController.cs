using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;

namespace OurBeautyReferralNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleRepo _roleRepo;

        public RoleController(RoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        [HttpGet("get-roles")]
        public ActionResult<IEnumerable<AspNetRole>> GetRoles()
        {
            var roles = _roleRepo.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            var result = await _roleRepo.AddRole(roleName);
            return result;
        }
    }
}
