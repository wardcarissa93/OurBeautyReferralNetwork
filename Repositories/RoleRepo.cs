using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _context;

        public RoleRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AspNetRole> GetAllRoles()
        {
            return _context.Roles.Select(r => new AspNetRole { 
                Id = r.Id,
                Name = r.Name,
                NormalizedName = r.NormalizedName,
                ConcurrencyStamp = r.ConcurrencyStamp
            });
        }

        public AspNetRole GetRole(string roleName)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            return role != null ? new AspNetRole
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                ConcurrencyStamp = role.ConcurrencyStamp
            } : null;
        }

        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                var normalizedRoleName = roleName.ToUpper();
                var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName);
                if (existingRole != null)
                {
                    return new BadRequestObjectResult("Role already exists in database.");
                }

                var newRole = new IdentityRole
                {
                    Name = roleName,
                    NormalizedName = normalizedRoleName
                };

                _context.Roles.Add(newRole);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Role added successfully.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error adding role: {ex.Message}");
            }
        }
    }
}
