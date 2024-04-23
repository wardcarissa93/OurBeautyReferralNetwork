using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class DiscountRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public DiscountRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Discount> GetAllDiscounts()
        {
            return _obrnContext.Discounts.ToList();
        }

        public Discount GetDiscountById(string discountId)
        {
            var discount = _obrnContext.Discounts.FirstOrDefault(t => t.PkDiscountId == discountId);
            if (discount == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return discount;
        }

    }
}
