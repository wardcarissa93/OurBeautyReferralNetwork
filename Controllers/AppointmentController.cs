using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OurBeautyReferralNetwork.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        private readonly obrnDbContext _obrnContext;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public AppointmentController(ApplicationDbContext context, obrnDbContext obrnContext,
                              UserManager<IdentityUser> userManager,
                              IConfiguration configuration)
        {
            _obrnContext = obrnContext;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/appointment/{appointmentId}")]

        public virtual IActionResult GetAppointmentById([FromRoute][Required] int appointmentId)
        {
            AppointmentRepo appointmentRepo = new AppointmentRepo(_context, _obrnContext);
            var appointment = appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return NotFound(); // Return a 404 Not Found response if feeId does not exist
            }
            return Ok(appointment);
        }

        [HttpGet]
        [Route("/appointment")]
        //[ValidateModelState]
        [SwaggerOperation("AppointmentGet")]
        public virtual IActionResult AppointmentGet()
        {
            AppointmentRepo appointmentRepo = new AppointmentRepo(_context, _obrnContext);
            var appointments = appointmentRepo.GetAllAppointments();
            return Ok(appointments);
        }
    }
}
