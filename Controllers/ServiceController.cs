using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.DataTransferObjects;
using OurBeautyReferralNetwork.EntityExtensions;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace OurBeautyReferralNetwork.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly BusinessRepo _businessRepo; // Add BusinessRepo as a dependency

        public ServiceController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration, BusinessRepo businessRepo)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
          _businessRepo = businessRepo; // Inject BusinessRepo

        }

        [HttpGet]
        [Route("/service/{serviceId}")]

        public virtual IActionResult GetServiceById([FromRoute][Required] int serviceId)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext, _businessRepo);

            var service = serviceRepo.GetServiceById(serviceId);
            if (service == null)
            {
                return NotFound(); // Return a 404 Not Found response if testimonialId does not exist
            }
            decimal? discountPercentage = serviceRepo.GetServiceDiscount(serviceId);
            decimal actualDiscount = discountPercentage ?? 0;
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            Discount discount = discountRepo.GetDiscountById(service.FkDiscountId);

            var extendedService = service.ExtendService(discount);
            return Ok(extendedService);
        }

        [HttpGet]
        [Route("/services")]
        //[ValidateModelState]
        [SwaggerOperation("ServiceGetAll")]
        public virtual IActionResult ServiceGetAll()
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext, _businessRepo);
            var services = serviceRepo.GetAllServices();
            return Ok(services);
        }

        [HttpGet]
        [Route("/service/business/{businessId}")]
        //[ValidateModelState]
        [SwaggerOperation("GetServicesForBusiness")]
        public virtual IActionResult GetServicesForBusiness(string businessId)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext, _businessRepo);
            var services = serviceRepo.GetAllServicesOfBusiness(businessId);
            return Ok(services);
        }

        [HttpGet]
        [Route("/service/{businessId}/{serviceId}")]
        //[ValidateModelState]
        [SwaggerOperation("GetServiceForBusiness")]
        public virtual IActionResult GetServiceForBusiness(string businessId, int serviceId)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext, _businessRepo);
            var services = serviceRepo.GetAllServicesOfBusiness(businessId);
            return Ok(services);
        }

        [HttpPost]
        [Route("/service/create")]
        public IActionResult CreateForBusiness(ServiceCreateDTO serviceCreateDTO)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext, _businessRepo);
            DiscountRepo discountRepo = new DiscountRepo(_context, _obrnContext);
            var discount = discountRepo.GetDiscountById(serviceCreateDTO.FkDiscountId);
            if (discount == null)
            {
                return NotFound("Discount not found"); // Handle the case where discount is not found
            }
            Service createdService = serviceRepo.CreateServiceForBusiness(serviceCreateDTO, discount);

            if (createdService != null)
            {
                return CreatedAtAction(nameof(GetServicesForBusiness), new { businessId = serviceCreateDTO.FkBusinessId }, createdService);
            }
            return BadRequest("Failed to create the service");

        }

        [HttpPut("{serviceId}")]
        public IActionResult Update (int serviceId, ServiceDTO serviceDTO)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext, _businessRepo);
            bool isSuccess = serviceRepo.EditServiceForBusinessAsync(serviceDTO, serviceId);
            if (!isSuccess)
            {
                return NotFound("Service not found with the provided ID.");
            }
            return Ok();
        }

        [HttpDelete("{serviceId}")]
        [SwaggerOperation("Delete")]
        public IActionResult Delete(int serviceId)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext, _businessRepo);
            string message = serviceRepo.Delete(serviceId);
            if (message == "Discount does not exist")
            {
                return NotFound();
            }
            else if (message == "Deleted successfully")
            {
                return Ok(); // server successfully processed the request and there is no content to send in the response payload.
            }
            else
            {
                // Handle other potential error cases, such as database errors
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
