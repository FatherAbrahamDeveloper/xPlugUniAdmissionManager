using XplugUniAdmissionManager.Models.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class CourseRegVM
{
    public string PhotoPath { get; set; } = Empty;
    public string? MatricNo { get; set; }
    public int FacultyId { get; set; }
    public Faculty Faculty {  get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public int CourseofStudyId { get; set; }
    public CourseOfStudy CourseOfStudy { get; set;}
}
