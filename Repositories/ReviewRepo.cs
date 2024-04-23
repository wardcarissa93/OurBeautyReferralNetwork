using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

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

    }
}
