using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
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

        [HttpPost("add-user-role/{email}")]
        public async Task<IActionResult> AddUserRole(string email, UserRoleDTO userRoleDTO)
        {
            var result = await _userRoleRepo.AddUserRoleAsync(email, userRoleDTO);
            return result;
        }

        [HttpGet("get-user-roles/{email}")]
        public async Task<IList<string>> GetUserRoles(string email)
        {
            var roles = await _userRoleRepo.GetUserRolesAsync(email);
            return roles;
        }
    }
}
