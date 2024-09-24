using xPlugUniAdmissionManager.Assets.Enums;

namespace xPlugUniAdmissionManager.Models.RegVMs.Core
{
    public class ModuleHelpVM
    {
        public string CurrentServiceUrl { get; set; } = Empty;
        public string PrevServiceUrl { get; set; } = Empty;
        public string NextServiceUrl { get; set; } = Empty;
        public string FormId { get; set; } = Empty;
        public RegStage PrevStage { get; set; }
        public RegStage CurrentStage { get; set; }
        public RegStage NextStage { get; set; }
        //public RegType RegType { get; set; }
    }


    public class ModuleIDVM
    {
        public int FacultyId { get; set; }
        public int StateId { get; set; }
        public int SelLocalAreaId { get; set; }
        public int SelCourseId { get; set; }
        public int InstType { get; set; }
        public int SelInstId { get; set; }

    }
}
