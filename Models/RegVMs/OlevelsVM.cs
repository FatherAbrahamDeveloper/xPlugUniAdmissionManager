using xPlugUniAdmissionManager.Models.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class OlevelsVM
{
    public string PhotoPath { get; set; } = Empty;
    public ExamBody  ExamBody { get; set; }
    public ExamType ExamType { get; set; }
    public string? ExamYear { get; set; } 
    public ExamSubject Subject { get; set; }
    public ExamGrade Grade { get; set; }
    
}
