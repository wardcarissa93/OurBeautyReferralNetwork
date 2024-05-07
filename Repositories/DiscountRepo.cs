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

        public bool CreateDiscount(Discount discount)
        {
            bool isSuccess = true;
            try
            {
                _obrnContext.Discounts.Add(new Discount
                {
                    PkDiscountId = discount.PkDiscountId,
                    Percentage = discount.Percentage,
                    StartDate = discount.StartDate,
                    EndDate = discount.EndDate,
                });
                _obrnContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public string Delete(string discountId)
        {
            try
            {
                var discount = GetDiscountById(discountId);
                if (discount == null)
                {
                    return "Fee does not exist";
                }

                if (_obrnContext.Services.Any(s => s.FkDiscountId == discountId))
                {
                    return "this discount is currently in use";
                }
                _obrnContext.Discounts.Remove(discount);
                _obrnContext.SaveChanges();
                return "Deleted successfully";
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // You can also return a custom error message if needed
                Console.WriteLine($"Error occurred during delete: {ex.Message}");
                return "An error occurred during delete";
            }
        }

    }
}
