namespace xPlugUniAdmissionManager.Models.RegVMs;

public class DocUploadsVM
{
    public string PhotoPath { get; set; } = Empty;
    public bool BirthCertificate { get; set; }
    public bool Testimonial { get; set; }
    public bool AdmissionLetter { get; set; }
    public bool Olevel { get; set; }
    public bool Olevel2 { get; set; }
    public bool UTME { get; set; }
    public bool NIN { get; set; }
    public bool ReferenceLetter { get; set; }

}
