using XplugUniAdmissionManager.Models.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class CourseRegVM
{
    public string PhotoPath { get; set; } = Empty;
    public string? MatricNumber { get; set; }
    public Faculty Faculty {  get; set; }
    public Department Department { get; set; }
    public CourseOfStudy CourseOfStudy { get; set;}
}
