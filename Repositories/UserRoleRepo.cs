﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class UserRoleRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRoleRepo(ApplicationDbContext context,
                            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddUserRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var hasRole = await _userManager.IsInRoleAsync(user, roleName);
                if (hasRole)
                {
                    return new BadRequestObjectResult($"User {email} already has the role {roleName}.");
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return new OkObjectResult("Role assigned to user successfully.");
                }

                return new BadRequestObjectResult("Failed to add role to user.");
            }

            return new BadRequestObjectResult("User not found.");
        }

        public async Task<IList<string>> GetUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles;
            }
            return null;
        }
    }
}
