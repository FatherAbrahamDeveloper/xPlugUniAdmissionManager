$("#StateId").on("change", function () {
    const id = $("#StateId").val();
    if (isEmpty(id) || parseInt(id) < 1) return false;
    const lName = isEmpty($("#StateId option:selected").text()) ? "" : $("#StateId option:selected").text();
    $("#StateName").val(lName)
    loadLGAs(id, 0);
});

function loadLGAs(id, selId) {

    var items = "";
    items = "<option>Loading ...</option>";
    $("#LocalAreaId").html("");
    $("#LocalAreaId").html(items);
    $("#LocalAreaId").attr("disabled", "disabled");

    const catUrl = '/lookup-data/load-lgas?stateId=' + parseInt(id) + '&selectedId=' + parseInt(selId);
    $.ajax({
        type: "GET",
        url: catUrl,
        dataType: "html",
        success: function (data) {
            $('#ddlLocalArea').html("");
            $('#ddlLocalArea').html(data);
        }
    })
}
$(document).on("change", "#LocalAreaId", function () {
    const id = $("#LocalAreaId").val();
    if (isEmpty(id) || parseInt(id) < 1) return false;
    const lName = isEmpty($("#LocalAreaId option:selected").text()) ? "" : $("#LocalAreaId option:selected").text();
    $("#LocalAreaName").val(lName)
});

$("#InstitutionType").on("change", function () {
    var id = $("#InstitutionType").val();
    if (isEmpty(id) || parseInt(id) < 1) return false;
    loadInstitutions(id, 0);
});

$(document).on("change", "#InstitutionId", function () {
    const id = $("#InstitutionId").val();
    if (isEmpty(id) || parseInt(id) < 1) return false;
    const lName = isEmpty($("#InstitutionId option:selected").text()) ? "" : $("#InstitutionId option:selected").text();
    $("#InstitutionName").val(lName)
});

function loadInstitutions(id, selId) {

    var items = "";
    items = "<option>Loading ...</option>";
    $("#InstitutionId").html("");
    $("#InstitutionId").html(items);
    $("#InstitutionId").attr("disabled", "disabled");

    const catUrl = '/lookup-data/load-institutions?instType=' + parseInt(id) + '&selectedId=' + parseInt(selId);

    $.ajax({
        type: "GET",
        url: catUrl,
        dataType: "html",
        success: function (data) {
            $('#ddlInstitution').html("");
            $('#ddlInstitution').html(data);
        }
    })
}

$("#FacultyId").on("change", function () {
    var id = $("#FacultyId").val();
    if (isEmpty(id) || parseInt(id) < 1) return false;
    const lName = isEmpty($("#FacultyId option:selected").text()) ? "" : $("#FacultyId option:selected").text();
    $("#FacultyName").val(lName)
    loadCourses(id, 0);
});

$(document).on("change", "#CourseId", function () {
    const id = $("#CourseId").val();
    if (isEmpty(id) || parseInt(id) < 1) return false;
    const lName = isEmpty($("#CourseId option:selected").text()) ? "" : $("#CourseId option:selected").text();
    $("#CourseName").val(lName)
});


function loadCourses(id, selId) {

    var items = "";
    items = "<option>Loading ...</option>";
    $("#CourseId").html("");
    $("#CourseId").html(items);
    $("#CourseId").attr("disabled", "disabled");

    const catUrl = '/lookup-data/load-courses?facultyId=' + parseInt(id) + '&selectedId=' + parseInt(selId);
    $.ajax({
        type: "GET",
        url: catUrl,
        dataType: "html",
        success: function (data) {
            $('#ddlCourse').html("");
            $('#ddlCourse').html(data);
        }
    })
}

function onloadCascade(instType, selInstId, facultyId, selCourseId, stateId, selLgadId) {
    const inType = parseInt(instType);
    const facId = parseInt(facultyId);
    const stId = parseInt(stateId);
    const lgaId = parseInt(selLgadId);
    const insId = parseInt(selInstId);
    const courseId = parseInt(selCourseId);

    if (inType > 0) {
        loadInstitutions(inType, insId);
    }

    if (facId > 0) {
        loadCourses(facId, courseId);
    }

    if (stId > 0) {
        loadLGAs(stId, lgaId);
    }
}