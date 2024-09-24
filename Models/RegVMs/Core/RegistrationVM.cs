using xPlugUniAdmissionManager.Assets.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class RegistrationVM
{
    public Guid ApplicationId { get; set; } = default!;
    public string PhotoPath { get; set; } = Empty;
    public bool IsSubmitted { get; set; }
    public RegStage RegStage { get; set; }
    //public RegType RegType { get; set; }
    public BioDataVM BioData { get; set; } = default!;
    public ContactVM Contact { get; set; } = default!;
    public CourseRegVM CourseReg { get; set; } = default!;
    public LibraryVM Library { get; set; } = default!;
    public MedicalsVM Medicals { get; set; } = default!;
    public NokInfoVM NokInfo { get; set; } = default!;
    public OlevelsVM Olevels { get; set; } = default!;
    public DocUploadsVM DocuUploads { get; set; } = default!;

}
