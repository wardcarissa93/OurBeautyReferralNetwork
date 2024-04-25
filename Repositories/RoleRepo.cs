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
    }
}
