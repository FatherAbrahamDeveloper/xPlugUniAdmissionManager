using Microsoft.AspNetCore.Mvc;

namespace xPlugUniAdmissionManager.Controllers.Datas;
[Route("lookup-data")]
public class LookupDataController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    #region Cascade
    [HttpGet("load-lgas")]
    public IActionResult _LoadLocalAreaComponent(int stateId, int selectedId)
    {
        return ViewComponent("LocalAreaDDL", new { stateId, selectedId });
    }

    //[HttpGet("load-institutions")]
    //public IActionResult _LoadInstitutionsComponent(int instType, int selectedId)
    //{
    //    return ViewComponent("InstitutionDDL", new { instType, selectedId });
    //}

    [HttpGet("load-courses")]
    public IActionResult _LoadCourseComponent(int facultyId, int selectedId)
    {
        return ViewComponent("CourseDDL", new { facultyId, selectedId });
    }
    #endregion
}
