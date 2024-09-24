namespace xPlugUniAdmissionManager.Models.RegVMs
{
    public class ContactVM
    {
        public string PhotoPath { get; set; } = Empty;
        public string MobileNo { get; set; } = Empty;
        public string Email { get; set; } = Empty;
        public string AltMobileNo { get; set; } = Empty;
        public string AltEmail { get; set; } = Empty;
        //Residential Address
        public string ResHouseNo { get; set; } = Empty;
        public string ResStreet { get; set; } = Empty;
        public string ResArea { get; set; } = Empty;
        public string ResCity { get; set; } = Empty;
        public string ResLandMark { get; set; } = Empty;
        public int ResLocalAreaId { get; set; }
        public int ResStateId { get; set; }
        public string ResStateName { get; set; } = Empty;
        public string ResLocalAreaName { get; set; } = Empty;

        //Permanent Address
        public string PermHouseNo { get; set; } = Empty;
        public string PermStreet { get; set; } = Empty;
        public string PermArea { get; set; } = Empty;
        public string PermCity { get; set; } = Empty;
        public string PermLandMark { get; set; } = Empty;
        public int PermLocalAreaId { get; set; }
        public int PermStateId { get; set; }
        public string PermStateName { get; set; } = Empty;
        public string PermLocalAreaName { get; set; } = Empty;
    }
}
