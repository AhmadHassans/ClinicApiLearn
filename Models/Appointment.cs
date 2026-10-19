namespace ClinicApiLearn.Models;

public class Appointment:BaseEntity
{
    public string  PatientName { get; set; }=string.Empty;
    public string Reason{ get; set; } =string.Empty;
    public DateTime AppointmentDate { get; set; } 
    public string? Prescription { get; set; }

    public int? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
}
