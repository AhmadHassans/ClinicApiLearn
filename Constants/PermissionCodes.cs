namespace ClinicApiLearn.Constants;

public static class PermissionCodes
{
    public const string AppointmentRead = "Appointment.Read";
    public const string AppointmentCreate = "Appointment.Create";
    public const string AppointmentUpdate = "Appointment.Update";
    public const string AppointmentDelete = "Appointment.Delete";
    public const string PrescriptionWrite = "Prescription.Write";
    public const string UserManage = "User.Manage";

    public static readonly string[] All =
    {
        AppointmentRead, AppointmentCreate, AppointmentUpdate,
        AppointmentDelete, PrescriptionWrite, UserManage
    };
}