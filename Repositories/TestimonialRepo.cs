using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class TestimonialRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public TestimonialRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Testimonial> GetAllTestimonials()
        {
            return _obrnContext.Testimonials.ToList();
        }

        public Testimonial GetTestimonialById(int testimonialId)
        {
            var testimonial = _obrnContext.Testimonials.FirstOrDefault(t => t.PkTestimonialId == testimonialId);
            if (testimonial == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return testimonial;
        }

    }
}
