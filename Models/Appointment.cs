using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurBeautyReferralNetwork.Models;

public partial class Appointment
{
    [Column("pkAppointmentID")]

    public int Pkappointmentid { get; set; }

    [Column("fkCustomerID")]

    public int Fkcustomerid { get; set; }

    [Column("AppointmentDate")]

    public DateOnly Appointmentdate { get; set; }

    [Column("AppointmentTime")]

    public TimeOnly Appointmenttime { get; set; }

    [Column("Referred")]

    public char Referred { get; set; }

    [Column("fkServiceID")]

    public int? Fkserviceid { get; set; }

    public virtual ICollection<Appointmentservice> Appointmentservices { get; set; } = new List<Appointmentservice>();

    public virtual Customer Fkcustomer { get; set; } = null!;

    public virtual Appointmentservice? Fkservice { get; set; }
}
