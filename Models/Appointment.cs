using System;
using System.Collections.Generic;

namespace OurBeautyReferralNetwork.Models;

public partial class Appointment
{
    public int Pkappointmentid { get; set; }

    public string Fkcustomerid { get; set; }

    public DateOnly Appointmentdate { get; set; }

    public TimeOnly Appointmenttime { get; set; }

    public char Paymentstatus { get; set; }

    public int? Fkserviceid { get; set; }

    public virtual ICollection<Appointmentservice> Appointmentservices { get; set; } = new List<Appointmentservice>();

    public virtual Customer Fkcustomer { get; set; } = null!;

    public virtual Appointmentservice? Fkservice { get; set; }
}
