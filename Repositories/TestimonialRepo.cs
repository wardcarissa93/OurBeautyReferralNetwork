using Microsoft.AspNetCore.Mvc;
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

        public bool CreateTestimonial(Testimonial testimonial)
        {
            bool isSuccess = true;
            try
            {
                _obrnContext.Testimonials.Add(new Testimonial
                {
                    PkTestimonialId = testimonial.PkTestimonialId,
                    FkBusinessId = testimonial.FkBusinessId,
                    Description = testimonial.Description,
                    Rating = testimonial.Rating,
                    TestimonialDate = DateOnly.FromDateTime(DateTime.Now)
            }); 
                _obrnContext.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public string Delete(int TestimonialId)
        {
            try
            {
                var testimonial = GetTestimonialById(TestimonialId);
                if (testimonial == null)
                {
                    return "Testimonial does not exist";
                }

                _obrnContext.Testimonials.Remove(testimonial);
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
