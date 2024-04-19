using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Appointmentservice
{
    [Column("pkAppointmentServiceID")]

    public int Pkappointmentserviceid { get; set; }

    [Column("fkAppointmentID")]

    public int? Fkappointmentid { get; set; }

    [Column("fkServiceID")]

    public int? Fkserviceid { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [Column("fk_appointment_id")]

    public virtual Appointment? Fkappointment { get; set; }
}
