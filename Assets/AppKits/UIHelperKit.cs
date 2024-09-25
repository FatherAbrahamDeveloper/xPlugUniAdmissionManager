using xPlugUniAdmissionManager.Assets.Enums;
using xPlugUniAdmissionManager.Models.RegVMs.Core;

namespace xPlugUniAdmissionManager.Assets;

public class UIHelperKit
{
    //public static (string title, string csClass, string iiCon, string btnTheme) WizHeading(RegType regType)
    //{
    //    return regType switch
    //    {
    //        RegType.SWIES => ("Industrial Attachment", "innerheaderbg", "siwes-i.png", "reg"),
    //        RegType.NYSC => ("Level-Up Skill", "innerheaderbg1", "nysc-i.png", "reg1"),
    //        RegType.Starter => ("IT Starter Registration", "innerheaderbg2", "ssce-i.png", "reg2"),
    //        RegType.Intern => ("Intern Registration", "innerheaderbg3", "intern-i.png", "reg3"),
    //        _ => ("SIWES Registration", "innerheaderbg4", "siwes-i.png", "reg"),
    //    };
    //}

    public static string WizFormId(RegStage stage)
    {
        switch (stage)
        {
            case RegStage.BioData:
                return "#frmBioData";
            case RegStage.Contact:
                return "#frmContact";
            case RegStage.NokInfo:
                return "#frmNokInfo";
            case RegStage.Medicals:
                return "#frmMedicals";
            case RegStage.Olevels:
                return "#frmOlevels";
            case RegStage.Library:
                return "#frmLibrary";
            case RegStage.CourseReg:
                return "#frmCourse";
            case RegStage.DocUploads:
                return "#frmDocumentUploads";
            default:
                return "#frmBioData";
        }
    }

    public static (string moduleName, object model) WizModule(RegistrationVM regModel)
    {
        var stage = regModel.RegStage;
        switch (stage)
        {
            case RegStage.BioData:
                return ("_BioData", regModel.BioData);
            case RegStage.Contact:
                return ("_Contact", regModel.Contact);
            case RegStage.NokInfo:
                return ("_NokInfo", regModel.NokInfo);
            case RegStage.Medicals:
                return ("_Medicals", regModel.Medicals);
            case RegStage.Olevels:
                return ("_Olevels", regModel.Olevels);
            case RegStage.Library:
                return ("_Library", regModel.Library);
            case RegStage.CourseReg:
                return ("_CourseRegistration", regModel.CourseReg);
            case RegStage.DocUploads:
                return ("_DocUploads", regModel.DocUploads);
            default:
                return ("_BioData", regModel.BioData);
        }
    }

    public static ModuleIDVM WizIdHelp(RegistrationVM regModel)
    {
        var item = new ModuleIDVM();
        //if (regModel.Education != null)
        //{
        //    item.FacultyId = regModel.Education.FacultyId;
        //    item.InstType = (int)regModel.Education.InstitutionType;
        //    item.SelInstId = regModel.Education.InstitutionId;
        //    item.SelCourseId = regModel.Education.CourseId;
        //}

        if (regModel.Contact != null)
        {
            item.StateId = regModel.Contact.ResStateId;
            item.SelLocalAreaId = regModel.Contact.ResLocalAreaId;
        }

        return item;
    }
    public static ModuleHelpVM WizModuleHelp(RegistrationVM regModel)
    {
        var stage = regModel.RegStage;

        switch (stage)
        {
            case RegStage.BioData:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-biodata",
                    CurrentStage = stage,
                    FormId = "#frmBiodata",
                    NextServiceUrl = "/register/process-contact",
                    NextStage = RegStage.Contact,
                    PrevServiceUrl = "",
                    PrevStage = stage,
                };
            case RegStage.Contact:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-contact",
                    CurrentStage = stage,
                    FormId = "#frmContact",
                    NextServiceUrl = "/register/process-education",
                    NextStage = RegStage.NokInfo,
                    PrevServiceUrl = "/register/process-biodata",
                    PrevStage = RegStage.BioData,
                };
            case RegStage.NokInfo:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-next-of-kin",
                    CurrentStage = stage,
                    FormId = "#frmNokInfo",
                    NextServiceUrl = "/register/process-medicals",
                    NextStage = RegStage.Medicals,
                    PrevServiceUrl = "/register/process-contact",
                    PrevStage = RegStage.Contact,
                };
            case RegStage.Medicals:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-medicals",
                    CurrentStage = stage,
                    FormId = "#frmMedicals",
                    NextServiceUrl = "/registerprocess-o-levels",
                    NextStage = RegStage.Olevels,
                    PrevServiceUrl = "/register/process-next-of-kin",
                    PrevStage = RegStage.NokInfo,
                };
            case RegStage.Olevels:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-o-levels",
                    CurrentStage = stage,
                    FormId = "#frmOlevels",
                    NextServiceUrl = "/register/process-library",
                    NextStage = RegStage.Library,
                    PrevServiceUrl = "/register/process-medicals",
                    PrevStage = RegStage.Medicals,
                };
            case RegStage.Library:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-library",
                    CurrentStage = stage,
                    FormId = "#frmLibrary",
                    NextServiceUrl = "/register/process-course-registration",
                    NextStage = RegStage.CourseReg,
                    PrevServiceUrl = "/register/process-o-levels",
                    PrevStage = RegStage.Olevels,
                };
            case RegStage.CourseReg:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-course-registration",
                    CurrentStage = stage,
                    FormId = "#frmCourse",
                    NextServiceUrl = "/register/process-document-uploads",
                    NextStage = RegStage.DocUploads,
                    PrevServiceUrl = "/register/process-library",
                    PrevStage = RegStage.Library,
                };
            case RegStage.DocUploads:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-document-uploads",
                    CurrentStage = stage,
                    FormId = "#frmDocumentUploads",
                    NextServiceUrl = "/register/app-summary",
                    NextStage = RegStage.Submission,
                    PrevServiceUrl = "/register/process-course-registration",
                    PrevStage = RegStage.CourseReg,
                };

            default:
                return new ModuleHelpVM
                {
                    CurrentServiceUrl = "/register/process-biodata",
                    CurrentStage = stage,
                    FormId = "#frmBiodata",
                    NextServiceUrl = "/register/process-contact",
                    NextStage = RegStage.Contact,
                    PrevServiceUrl = "",
                    PrevStage = stage,
                    // study here carefully
                };
        }
    }
   
}
