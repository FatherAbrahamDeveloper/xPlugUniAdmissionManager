using xPlugUniAdmissionManager.Models.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class MedicalsVM
{
    public string PhotoPath { get; set; } = Empty;
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public decimal BMI { get; private set; } //figure will be automatic
    public BloodGroup BloodGroup { get; set; }
    public Genotype Genotype { get; set; }
    public bool Allergy { get; set; }
    public string? AllergyType { get; set; }
    public bool MedicalCondition { get; set; }
    public string? MedicalConditionType { get; set; }
}
