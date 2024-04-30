using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.Models;
using static System.Net.Mime.MediaTypeNames;

namespace OurBeautyReferralNetwork.Repositories
{
    public class ReviewRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public ReviewRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return _obrnContext.Reviews.ToList();
        }

        public Review GetReviewById(int reviewId)
        {
            var review = _obrnContext.Reviews.FirstOrDefault(t => t.PkReviewId == reviewId);
            if (review == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return review;
        }

        public IEnumerable<Review> GetAllReviewsForBusiness(string businessId)
        {
            var allReviews = _obrnContext.Reviews
                .Where(r => r.FkBusinessId == businessId)
                .ToList();

            return allReviews;
        }

        public Review CreateReviewForBusiness(ReviewDTO reviewDTO,string businessId)
        {
            var review = new Review
            {
                PkReviewId = reviewDTO.PkReviewId,
                FkBusinessId = reviewDTO.FkBusinessId,
                FkCustomerId = reviewDTO.FkCustomerId,
                Description = reviewDTO.Description,
                Rating = reviewDTO.Rating,
                ReviewDate = DateOnly.FromDateTime(DateTime.Now),
                Image = reviewDTO.Image,
                Provider = reviewDTO.Provider,
            };
            try
            {
                _obrnContext.Reviews.Add(review);
                _obrnContext.SaveChanges();
                return review;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public string Delete(int reviewId)
        {
            try
            {
                var review = GetReviewById(reviewId);
                if (review == null)
                {
                    return "Review does not exist";
                }
                _obrnContext.Reviews.Remove(review);
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
