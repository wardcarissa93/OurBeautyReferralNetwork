using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Appointmentservice
{
    public int Pkappointmentserviceid { get; set; }

    public int? Fkappointmentid { get; set; }

    public int? Fkserviceid { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Appointment? Fkappointment { get; set; }
}
