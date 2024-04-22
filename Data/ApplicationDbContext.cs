using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {        
        }
        // public DbSet<Customer> Customers { get; set; }
    }
}
