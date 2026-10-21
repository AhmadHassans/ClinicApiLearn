namespace ClinicApiLearn.Models;

public class Appointment:BaseEntity
{
    public required string PatientName { get; set; }
    public required string Reason { get; set; }
    public DateTime AppointmentDate { get; set; } 
    public string? Prescription { get; set; }

    public Guid? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
}
