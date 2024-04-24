using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class CategoryRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public CategoryRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _obrnContext.Categories.ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            var category = _obrnContext.Categories.FirstOrDefault(c => c.PkCategoryId == categoryId);
            if (category == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return category;
        }

    }
}
