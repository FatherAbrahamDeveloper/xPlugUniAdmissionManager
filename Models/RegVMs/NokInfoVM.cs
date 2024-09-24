using xPlugUniAdmissionManager.Models.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class NokInfoVM
{
    public string PhotoPath { get; set; } = Empty;
    public string NokSurname { get; set; } = Empty;
    public string NokFirstName { get; set; } = Empty;
    public string NokOtherNames { get; set; } = Empty;
    public DateTime NokDateOfBirth { get; set; } 
    public Gender NokGender { get; set; }
    public MaritalStatus NokMaritalStatus { get; set; }
    public string NokEmail { get; set; } = Empty;
    public string NokMobile { get; set; } = Empty;
    public NextOfKinType RelationshipType { get; set; }

    //Next of Kin Residential Address
    public string NokHouseNo { get; set; } = Empty;
    public string NokStreet { get; set; } = Empty;
    public string NokArea { get; set; } = Empty;
    public string NokCity { get; set; } = Empty;
    public string NokLandMark { get; set; } = Empty;
    public int NokLocalAreaId { get; set; }
    public int NokStateId { get; set; }
    public string NokStateName { get; set; } = Empty;
    public string NokLocalAreaName { get; set; } = Empty;
}
