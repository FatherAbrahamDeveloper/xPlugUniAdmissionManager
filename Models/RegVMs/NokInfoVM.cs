using xPlugUniAdmissionManager.Models.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs;

public class NokInfoVM
{
    public string PhotoPath { get; set; } = Empty;
    public string Surname { get; set; } = Empty;
    public string FirstName { get; set; } = Empty;
    public string OtherNames { get; set; } = Empty;
    public DateTime DateOfBirth { get; set; } 
    public Gender Gender { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public string Email { get; set; } = Empty;
    public string Mobile { get; set; } = Empty;
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
