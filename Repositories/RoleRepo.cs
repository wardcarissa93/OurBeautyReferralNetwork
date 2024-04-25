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

        public bool CreateRole(string roleName)
        {
            var normalizedRoleName = roleName.ToUpper();

            if (_context.Roles.Any(r => r.NormalizedName == normalizedRoleName))
            {
                return false; // Role already exists
            }

            _context.Roles.Add(new IdentityRole
            {
                Id = normalizedRoleName,
                Name = roleName,
                NormalizedName = normalizedRoleName
            });

            _context.SaveChanges();

            return true;
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
