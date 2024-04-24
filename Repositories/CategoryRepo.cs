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

        public bool CreateCategory(Category category)
        {
            bool isSuccess = true;
            try
            {
                _obrnContext.Categories.Add(new Category
                {
                    CategoryName = category.CategoryName,
                });
                _obrnContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public string Delete(int categoryId)
        {
            try
            {
                var category = GetCategoryById(categoryId);
                if (category == null)
                {
                    return "Fee does not exist";
                }

                if (_obrnContext.Services.Any(s => s.FkCategoryId == categoryId))
                {
                    return "this category is currently in use";
                }

                _obrnContext.Categories.Remove(category);
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
