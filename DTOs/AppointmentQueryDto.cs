using System.ComponentModel.DataAnnotations;

namespace ClinicApiLearn.DTOs;

public class AppointmentQueryDto
{
    // pagination
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    // searching (name||reason)
    [MaxLength(100)]
    public string? Search { get; set; }

    // filtering
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? HasPrescription { get; set; }

    // sorting: createdAt | date | patientName
    public string SortBy { get; set; } = "createdAt";

    // asc | desc (default: naya upar)
    public string SortDir { get; set; } = "desc";
}