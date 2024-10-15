namespace xPlugUniAdmissionManager.Models.RegVMs;

public class AddressVM
{
    public string HouseNo { get; set; } = Empty;
    public string Street { get; set; } = Empty;
    public string Area { get; set; } = Empty;
    public string City { get; set; } = Empty;
    public string LandMark { get; set; } = Empty;
    public int LocalAreaId { get; set; }
    public int StateId { get; set; }
    public string StateName { get; set; } = Empty;
    public string LocalAreaName { get; set; } = Empty;
}
