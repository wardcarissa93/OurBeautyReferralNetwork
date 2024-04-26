using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.EntityExtensions;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public ServiceController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/service/{serviceId}")]

        public virtual IActionResult GetServiceById([FromRoute][Required] int serviceId)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext);

            var service = serviceRepo.GetServiceById(serviceId);
            if (service == null)
            {
                return NotFound(); // Return a 404 Not Found response if testimonialId does not exist
            }
            decimal? discountPercentage = serviceRepo.GetServiceDiscount(serviceId);
            decimal actualDiscount = discountPercentage ?? 0;

            var extendedService = service.ExtendService(actualDiscount);
            return Ok(extendedService);
        }

        [HttpGet]
        [Route("/services")]
        //[ValidateModelState]
        [SwaggerOperation("ServiceGetAll")]
        public virtual IActionResult ServiceGetAll()
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext);
            var services = serviceRepo.GetAllServices();
            return Ok(services);
        }

        [HttpGet]
        //[Route("/service")]
        //[ValidateModelState]
        [SwaggerOperation("GetServicesForBusiness")]
        public virtual IActionResult GetServicesForBusiness(string businessId)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext);
            var services = serviceRepo.GetAllServicesOfBusiness(businessId);
            return Ok(services);
        }

        [HttpPost]
        [Route("/service/create")]
        public IActionResult CreateForBusiness(Service service, string businessID)
        {
            ServiceRepo serviceRepo = new ServiceRepo(_context, _obrnContext);
            bool isSuccess = serviceRepo.CreateServiceForBusiness(service, businessID);

            if (isSuccess)
            {
                return CreatedAtAction(nameof(GetServicesForBusiness), new { serviceId = service.PkServiceId }, service);
            }
            return BadRequest();

        }
    }
}
