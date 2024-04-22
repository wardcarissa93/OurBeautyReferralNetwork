using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Appointment
{
    public int PkAppointmentId { get; set; }

    public string FkCustomerId { get; set; } = null!;

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentTime { get; set; }

    public bool Referred { get; set; }

    public int? FkServiceId { get; set; }

    public virtual ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

    public virtual Customer FkCustomer { get; set; } = null!;

    public virtual AppointmentService? FkService { get; set; }
}
