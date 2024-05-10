using Microsoft.AspNetCore.Mvc;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
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

        public Testimonial GetTestimonialOfBusiness(string businessId)
        {
            var testimonial = _obrnContext.Testimonials.FirstOrDefault(t => t.FkBusinessId == businessId);
            if (testimonial == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return testimonial;
        }

        public Testimonial CreateTestimonial(TestimonialDTO testimonialDTO)
        {
            var testimonial = new Testimonial
            {
                FkBusinessId = testimonialDTO.FkBusinessId,
                Description = testimonialDTO.Description,
                Rating = testimonialDTO.Rating,
                TestimonialDate = DateOnly.FromDateTime(DateTime.Now),
                Approved = testimonialDTO.Approved,
            };
            try
            {
                _obrnContext.Testimonials.Add(testimonial);
                _obrnContext.SaveChanges();
                return testimonial; // Return the newly added testimonial
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return null; // Return null or handle the exception as needed
            }
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

        public string DeleteTestimonialOfBusiness(string businessId)
        {
            try
            {
                var testimonial = GetTestimonialOfBusiness(businessId);
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
