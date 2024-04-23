using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;

namespace OurBeautyReferralNetwork.Repositories
{
    public class AppointmentRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly obrnDbContext _obrnContext;

        public AppointmentRepo(ApplicationDbContext context, obrnDbContext obrnContext)
        {
            _context = context;
            _obrnContext = obrnContext;
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return _obrnContext.Appointments.ToList();
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            var appointment = _obrnContext.Appointments.FirstOrDefault(t => t.PkAppointmentId == appointmentId);
            if (appointment == null)
            {
                return null; // Return a 404 Not Found response if feeId does not exist
            }
            return appointment;
        }

    }
}
