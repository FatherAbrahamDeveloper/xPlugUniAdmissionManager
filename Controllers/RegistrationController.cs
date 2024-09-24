using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Text;
using xPlugUniAdmissionManager.Assets.AppKits;
using xPlugUniAdmissionManager.Assets.AppSession;
using xPlugUniAdmissionManager.Assets.AppSession.Core;
using xPlugUniAdmissionManager.Assets.Enums;

namespace xPlugUniAdmissionManager.Controllers;

public class RegistrationController(IValidator<BioDataVM> bioValidator, IValidator<ContactVM> contactValidator,
    IValidator<CourseRegVM> courseRegValidator, IValidator<DocUploadsVM> docUploadsValidator, IValidator<LibraryVM> libraryValidator, IValidator<MedicalsVM> medicalsValidator, IValidator<NokInfoVM> nokInfoValidator, IValidator<OlevelsVM> olevelsValidator,
    IStartSession sessionStarter, IWebHostEnvironment environment, ICacheUtil cacheUtil)
    : xPlugUniController(sessionStarter, cacheUtil)

{

    private readonly IValidator<BioDataVM> _bioValidator = bioValidator;
    private readonly IValidator<ContactVM> _contactValidator = contactValidator;
    private readonly IValidator<CourseRegVM> _courseRegValidator = courseRegValidator;
    private readonly IValidator<DocUploadsVM> _docUploadsValidator = docUploadsValidator;
    private readonly IValidator<LibraryVM> _libraryValidator = libraryValidator;
    private readonly IValidator<MedicalsVM> _medicalsValidator = medicalsValidator; 
    private readonly IValidator<NokInfoVM> _nokInfoValidator = nokInfoValidator;
    private readonly IValidator<OlevelsVM> _olevelsValidator = olevelsValidator;
    private readonly IWebHostEnvironment _environment = environment;
    private DirectoryHelpServ _directoryHelpServ = new(environment);

    private readonly StringBuilder _valErrors = new();

    //public IActionResult Index(int? progType)
    //{
    //    var calId = progType ?? 1;
    //    ViewBag.CallId = calId.ToString();
    //    return View();
    //}

    [HttpGet("wizard")]
    public IActionResult RegWizard( int? stage)
    {
        ViewBag.Error = "";
        var stageId = stage ?? (int)RegStage.BioData;

        var resp = GetRegFromStore();
        if (resp.Failed)
        {
            ViewBag.Error = resp.Errors[0];
            return View(new RegistrationVM());
        }
        var reg = resp.Value;
        reg.RegStage = (RegStage)stageId;
       

        return View(reg);
    }

    [HttpGet("app-summary")]
    public IActionResult AppSummary()
    {

        var resp = GetRegFromStore();
        if (resp.Failed)
        {
            ViewBag.Error = resp.Errors[0];
            return View(new RegistrationVM());
        }
        var reg = resp.Value;
        if (reg == null || reg.RegStage != RegStage.Submission)
        {
            ViewBag.Error = "Invalid Process Request";
            return View(new RegistrationVM());
        }

        return View(reg);
    }

    [HttpGet("app-submission")]
    public IActionResult AppSubmission()
    {

        //Get Save Data
        //Persist to database via API
        //Clear data session
        //redirect to portl dashboard

        ClearRegStore();

        return RedirectToActionPermanent("Index", "Home");
    }

    #region Upload Passport
    [HttpPost("upload-photo")]
    public JsonResult ProcessPhotoUpload()
    {
        try
        {
            var formFile = Request.Form.Files["photoUpload"];
            if (formFile == null)
            {
                return Json(new { IsSuccessful = false, Error = "Upload Failed! Try again later", IsAuthenticated = true });
            }

            var resp = GetRegFromStore();
            if (resp.Failed || resp.Value == null)
            {
                return Json(new { IsSuccessful = false, Error = "Invalid Session! Please Try again later", IsAuthenticated = true });
            }

            var regInfo = resp.Value;
            var filePath = _directoryHelpServ.SaveFile(formFile, regInfo.ApplicationId.ToString().Replace("-", "_"));
            if (IsNullOrEmpty(filePath) || filePath.Length < 5)
            {
                var msg = IsNullOrEmpty(_directoryHelpServ.MessageInfo) ? "Upload Failed! Please try again later" : _directoryHelpServ.MessageInfo;
                return Json(new { IsSuccessful = false, Error = msg, IsAuthenticated = true });
            }

            regInfo.PhotoPath = filePath;
            SaveRegToStore(regInfo);

            return Json(new { IsAuthenticated = true, IsSuccessful = true, PhotoPath = filePath, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }

    }
    #endregion

    [HttpPost("process-biodata")]
    public async Task<JsonResult> ProcessBiodata(BioDataVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _bioValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            reg.BioData ??= new BioDataVM();
            reg.BioData.PhotoPath = reg.PhotoPath;
            reg.BioData.Surname = model.Surname;
            reg.BioData.MaritalStatus = model.MaritalStatus;
            reg.BioData.DateOfBirth = model.DateOfBirth;
            reg.BioData.FirstName = model.FirstName;
            reg.BioData.HairColor = model.HairColor;
            reg.BioData.Gender = model.Gender;
            reg.BioData.EyeColor = model.EyeColor;
            reg.BioData.IsDisabled = model.IsDisabled;
            reg.BioData.DisabilityType = model.DisabilityType;
            reg.BioData.FacialMark = model.FacialMark;

            reg.Contact ??= new ContactVM();
            reg.Contact.PhotoPath = reg.PhotoPath;

            var result = SaveRegToStore(reg);
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = result.Errors[0], IsAuthenticated = true });

            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

    //Contact
    [HttpPost("process-contact")]
    public async Task<JsonResult> ProcessContact(ContactVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _contactValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            var valResult2 = await _bioValidator.ValidateAsync(reg.BioData);
            if (!valResult2.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult2.Errors.ToErrorListString(), IsAuthenticated = true });


            reg.Contact ??= new ContactVM();
            reg.Contact.PhotoPath = reg.PhotoPath;
            reg.Contact.MobileNo = model.MobileNo;
            reg.Contact.Email = model.Email;
            reg.Contact.AltMobileNo = model.AltMobileNo;
            reg.Contact.AltEmail = model.AltEmail;
            reg.Contact.ResHouseNo = model.ResHouseNo;
            reg.Contact.ResStreet = model.ResStreet;
            reg.Contact.ResArea = model.ResArea;
            reg.Contact.ResCity = model.ResCity;
            reg.Contact.ResLandMark = model.ResLandMark;
            reg.Contact.ResLocalAreaId = model.ResLocalAreaId;
            reg.Contact.ResStateId = model.ResStateId;
            reg.Contact.ResLocalAreaName = model.ResLocalAreaName;
            reg.Contact.PermHouseNo = model.PermHouseNo;
            reg.Contact.PermStreet = model.PermStreet;
            reg.Contact.PermArea = model.PermArea;
            reg.Contact.PermCity = model.PermCity;
            reg.Contact.PermLandMark = model.PermLandMark;
            reg.Contact.PermLocalAreaId = model.PermLocalAreaId;
            reg.Contact.PermLocalAreaName = model.PermLocalAreaName;
            reg.Contact.PermStateId = model.PermStateId;
            reg.Contact.PermStateName = model.PermStateName;

            reg.NokInfo ??= new NokInfoVM();
            reg.NokInfo.PhotoPath = reg.PhotoPath;


            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

    //Next of Kin Info
    [HttpPost("process-next-of-kin")]
    public async Task<JsonResult> ProcessNokInfo(NokInfoVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _nokInfoValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            if (reg.BioData == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Bio data Information", IsAuthenticated = true });

            var valResult2 = await _bioValidator.ValidateAsync(reg.BioData);
            if (!valResult2.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult2.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Contact == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Contact Information", IsAuthenticated = true });

            var valResult3 = await _contactValidator.ValidateAsync(reg.Contact);
            if (!valResult3.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult3.Errors.ToErrorListString(), IsAuthenticated = true });

            reg.NokInfo ??= new NokInfoVM();
            reg.NokInfo.PhotoPath = reg.PhotoPath;
            reg.NokInfo.NokSurname = model.NokSurname;
            reg.NokInfo.NokFirstName = model.NokFirstName;
            reg.NokInfo.NokOtherNames = model.NokOtherNames;
            reg.NokInfo.NokDateOfBirth = model.NokDateOfBirth;
            reg.NokInfo.NokGender = model.NokGender;
            reg.NokInfo.NokMaritalStatus = model.NokMaritalStatus;
            reg.NokInfo.NokEmail = model.NokEmail;
            reg.NokInfo.NokMobile = model.NokMobile;
            reg.NokInfo.RelationshipType = model.RelationshipType;
            reg.NokInfo.NokHouseNo = model.NokHouseNo;
            reg.NokInfo.NokStreet = model.NokStreet;
            reg.NokInfo.NokArea = model.NokArea;
            reg.NokInfo.NokCity = model.NokCity;
            reg.NokInfo.NokLandMark = model.NokLandMark;
            reg.NokInfo.NokLocalAreaId = model.NokLocalAreaId;
            reg.NokInfo.NokStateId = model.NokStateId;
            reg.NokInfo.NokStateName = model.NokStateName;
            reg.NokInfo.NokLocalAreaName = model.NokLocalAreaName;

            reg.Medicals ??= new MedicalsVM();
            reg.Medicals.PhotoPath = reg.PhotoPath;

            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

    //Medicals
    [HttpPost("process-medicals")]
    public async Task<JsonResult> ProcessMedicals(MedicalsVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _medicalsValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            if (reg.BioData == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Bio data Information", IsAuthenticated = true });

            var valResult2 = await _bioValidator.ValidateAsync(reg.BioData);
            if (!valResult2.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult2.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Contact == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Contact Information", IsAuthenticated = true });

            var valResult3 = await _contactValidator.ValidateAsync(reg.Contact);
            if (!valResult3.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult3.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.NokInfo == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Next of Kin Information", IsAuthenticated = true });

            var valResult4 = await _nokInfoValidator.ValidateAsync(reg.NokInfo);
            if (!valResult4.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult4.Errors.ToErrorListString(), IsAuthenticated = true });

            reg.Medicals ??= new MedicalsVM();
            reg.Medicals.PhotoPath = reg.PhotoPath;
            reg.Medicals.Weight = model.Weight;
            reg.Medicals.Height = model.Height;
            //reg.Medicals.BMI = model.BMI;
            reg.Medicals.Allergy = model.Allergy;
            reg.Medicals.AllergyType = model.AllergyType;
            reg.Medicals.MedicalCondition = model.MedicalCondition;
            reg.Medicals.MedicalConditionType = model.MedicalConditionType;

            reg.Olevels ??= new OlevelsVM();
            reg.Olevels.PhotoPath = reg.PhotoPath;


            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

    //O levels
    [HttpPost("process-o-levels")]
    public async Task<JsonResult> ProcessOlevels(OlevelsVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _olevelsValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            if (reg.BioData == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Bio data Information", IsAuthenticated = true });

            var valResult2 = await _bioValidator.ValidateAsync(reg.BioData);
            if (!valResult2.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult2.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Contact == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Contact Information", IsAuthenticated = true });

            var valResult3 = await _contactValidator.ValidateAsync(reg.Contact);
            if (!valResult3.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult3.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.NokInfo == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Next of Kin Information", IsAuthenticated = true });

            var valResult4 = await _nokInfoValidator.ValidateAsync(reg.NokInfo);
            if (!valResult4.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult4.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Medicals == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Medical Information", IsAuthenticated = true });

            var valResult5 = await _medicalsValidator.ValidateAsync(reg.Medicals);
            if (!valResult5.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            reg.Olevels ??= new OlevelsVM();
            reg.Olevels.PhotoPath = reg.PhotoPath;
            reg.Olevels.ExamBody = model.ExamBody;
            reg.Olevels.ExamType = model.ExamType;
            reg.Olevels.ExamYear = model.ExamYear;
            reg.Olevels.Subject = model.Subject;
            reg.Olevels.Grade = model.Grade;


            
            reg.Library ??= new LibraryVM();
            reg.Library.PhotoPath = reg.PhotoPath;


            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

    //Library
    [HttpPost("process-library")]
    public async Task<JsonResult> ProcessLibrary(LibraryVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _libraryValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            if (reg.BioData == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Bio data Information", IsAuthenticated = true });

            var valResult2 = await _bioValidator.ValidateAsync(reg.BioData);
            if (!valResult2.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult2.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Contact == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Contact Information", IsAuthenticated = true });

            var valResult3 = await _contactValidator.ValidateAsync(reg.Contact);
            if (!valResult3.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult3.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.NokInfo == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Next of Kin Information", IsAuthenticated = true });

            var valResult4 = await _nokInfoValidator.ValidateAsync(reg.NokInfo);
            if (!valResult4.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult4.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Medicals == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Medical Information", IsAuthenticated = true });

            var valResult5 = await _medicalsValidator.ValidateAsync(reg.Medicals);
            if (!valResult5.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });
            
            if (reg.Olevels == null)
                return Json(new { IsSuccessful = false, Error = "Invalid O'Levels Information", IsAuthenticated = true });

            var valResult6 = await _olevelsValidator.ValidateAsync(reg.Olevels);
            if (!valResult6.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            reg.Library ??= new LibraryVM();
            reg.Library.PhotoPath = reg.PhotoPath;
            reg.Library.LibraryNumber = model.LibraryNumber;
            reg.Library.MatricNumber = model.MatricNumber;
            reg.Library.AdmissionYear = model.AdmissionYear;
            reg.Library.LibraryUsername = model.LibraryUsername;


            reg.CourseReg ??= new CourseRegVM();
            reg.CourseReg.PhotoPath = reg.PhotoPath;

            //Next Stage
            //reg.OtherInfo ??= new OtherInfoVM();
            //reg.OtherInfo.PhotoPath = reg.PhotoPath;


            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

    //Course Registration
    [HttpPost("process-course-registration")]
    public async Task<JsonResult> ProcessCourseReg(CourseRegVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _courseRegValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            if (reg.BioData == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Bio data Information", IsAuthenticated = true });

            var valResult2 = await _bioValidator.ValidateAsync(reg.BioData);
            if (!valResult2.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult2.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Contact == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Contact Information", IsAuthenticated = true });

            var valResult3 = await _contactValidator.ValidateAsync(reg.Contact);
            if (!valResult3.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult3.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.NokInfo == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Next of Kin Information", IsAuthenticated = true });

            var valResult4 = await _nokInfoValidator.ValidateAsync(reg.NokInfo);
            if (!valResult4.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult4.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Medicals == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Medical Information", IsAuthenticated = true });

            var valResult5 = await _medicalsValidator.ValidateAsync(reg.Medicals);
            if (!valResult5.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Olevels == null)
                return Json(new { IsSuccessful = false, Error = "Invalid O'Levels Information", IsAuthenticated = true });

            var valResult6 = await _olevelsValidator.ValidateAsync(reg.Olevels);
            if (!valResult6.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });
            
            if (reg.Library == null)
                return Json(new { IsSuccessful = false, Error = "Invalid O'Levels Information", IsAuthenticated = true });

            var valResult7 = await _libraryValidator.ValidateAsync(reg.Library);
            if (!valResult7.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            reg.CourseReg ??= new CourseRegVM();
            reg.CourseReg.PhotoPath = reg.PhotoPath;
            reg.CourseReg.MatricNumber = model.MatricNumber;
            reg.CourseReg.Faculty = model.Faculty;
            reg.CourseReg.Department = model.Department;
            reg.CourseReg.CourseOfStudy = model.CourseOfStudy;


            reg.DocUploads ??= new DocUploadsVM();
            reg.DocUploads.PhotoPath = reg.PhotoPath;

            //Next Stage
            //reg.OtherInfo ??= new OtherInfoVM();
            //reg.OtherInfo.PhotoPath = reg.PhotoPath;


            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

    //Document Uploads
    [HttpPost("process-document-uploads")]
    public async Task<JsonResult> ProcessDocUploads(DocUploadsVM model)
    {
        try
        {

            if (model == null)
                return Json(new { IsSuccessful = false, Error = "Session Expired", IsAuthenticated = true });

            var valResult = await _docUploadsValidator.ValidateAsync(model);
            if (!valResult.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult.Errors.ToErrorListString(), IsAuthenticated = true });

            var resp = GetRegFromStore();
            if (resp.Failed)
                return Json(new { IsSuccessful = false, Error = resp.Errors[0], IsAuthenticated = true });

            var reg = resp.Value;

            if (reg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Application Session!", IsAuthenticated = true });

            if (IsNullOrEmpty(reg.PhotoPath) || reg.PhotoPath.Length < 6)
                return Json(new { IsSuccessful = false, Error = "Kindly attach your photograph", IsAuthenticated = true });

            if (reg.BioData == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Bio data Information", IsAuthenticated = true });

            var valResult2 = await _bioValidator.ValidateAsync(reg.BioData);
            if (!valResult2.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult2.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Contact == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Contact Information", IsAuthenticated = true });

            var valResult3 = await _contactValidator.ValidateAsync(reg.Contact);
            if (!valResult3.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult3.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.NokInfo == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Next of Kin Information", IsAuthenticated = true });

            var valResult4 = await _nokInfoValidator.ValidateAsync(reg.NokInfo);
            if (!valResult4.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult4.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Medicals == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Medical Information", IsAuthenticated = true });

            var valResult5 = await _medicalsValidator.ValidateAsync(reg.Medicals);
            if (!valResult5.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Olevels == null)
                return Json(new { IsSuccessful = false, Error = "Invalid O'Levels Information", IsAuthenticated = true });

            var valResult6 = await _olevelsValidator.ValidateAsync(reg.Olevels);
            if (!valResult6.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.Library == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Library Information", IsAuthenticated = true });

            var valResult7 = await _libraryValidator.ValidateAsync(reg.Library);
            if (!valResult7.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            if (reg.CourseReg == null)
                return Json(new { IsSuccessful = false, Error = "Invalid Course Registration Information", IsAuthenticated = true });

            var valResult8 = await _courseRegValidator.ValidateAsync(reg.CourseReg);
            if (!valResult7.IsValid)
                return Json(new { IsSuccessful = false, Error = valResult5.Errors.ToErrorListString(), IsAuthenticated = true });

            reg.DocUploads ??= new DocUploadsVM();
            reg.DocUploads.PhotoPath = reg.PhotoPath;
            reg.DocUploads.BirthCertificate = model.BirthCertificate;
            reg.DocUploads.Testimonial = model.Testimonial;
            reg.DocUploads.AdmissionLetter = model.AdmissionLetter;
            reg.DocUploads.Olevel = model.Olevel;
            reg.DocUploads.Olevel2 = model.Olevel2;
            reg.DocUploads.UTME = model.UTME;
            reg.DocUploads.NIN = model.NIN;
            reg.DocUploads.ReferenceLetter = model.ReferenceLetter;
            reg.RegStage = RegStage.Submission;

            //Next Stage
            //reg.DocUploads ??= new DocUploadsVM();
            //reg.DocUploads.PhotoPath = reg.PhotoPath;




            return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
        }
    }

}
