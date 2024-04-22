using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class AppointmentService
{
    public int PkAppointmentServiceId { get; set; }

    public int? FkAppointmentId { get; set; }

    public int? FkServiceId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Appointment? FkAppointment { get; set; }
}
