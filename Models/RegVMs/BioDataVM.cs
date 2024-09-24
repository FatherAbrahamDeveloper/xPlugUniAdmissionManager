using xPlugUniAdmissionManager.Models.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class BioDataVM
{
    public string PhotoPath { get; set; } = Empty;
    public string Surname { get; set; } = Empty;
    public string FirstName { get; set; } = Empty;
    public string OtherNames { get; set; } = Empty;
    public string DateOfBirth { get; set; } = DateTime.Today.AddYears(-18).ToString("yyyy-MM-dd");
    public string MaxDateOfBirth { get; set; } = DateTime.Now.AddYears(-18).ToString("yyyy-MM-dd");
    public Gender Gender { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public HairColor HairColor { get; set; }
    public EyeColor EyeColor { get; set; }
    public bool IsDisabled { get; set; }
    public DisabilityType DisabilityType { get; set; }
    public bool FacialMark { get; set; }

}
