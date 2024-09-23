$("#photoUpload").on('change', (function (e) {
    $("#photoView").attr('src', '/img/upload.gif');
    return doPhotoUpload();
}));
function doPhotoUpload() {

    var fileInput = document.getElementById('photoUpload');
    var file = fileInput.files[0];
    var formData = new FormData();
    formData.append('photoUpload', file);

    $.ajax({
        url: '/register/upload-photo',
        type: "POST",
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            unblockMainView();
            if (!data.IsAuthenticated) {
                ErrorToast("Upload Error! Try again", "dvError");
                return;
            }
            if (!data.IsSuccessful) {
                var retError = !isEmpty(data.Error) ? data.Error : "Unknown error occured. Please try again later!";
                ErrorToast(retError, "dvError");
                alert(retError);
                return;
            } else {
                $("#photoView").attr('src', '/AppDocs/TempUploads/' + data.PhotoPath);
                $("#frmPicUpload")[0].reset();
                return false;
            }
        },
        error: function (e) {
            $("#err").html(e).fadeIn();
        }
    });
}
function processNext(modInfo) {

    blockMainView();
    const frm = modInfo.FormId;
    const frmData = $(frm).serializeArray();
    const redUrl = "/register/wizard?progType=" + modInfo.RegType + "&stage=" + modInfo.NextStage;

    $.ajax({
        type: "POST",
        url: modInfo.CurrentServiceUrl,
        data: frmData,
        success: function (data) {
            unblockMainView();
            if (!data.IsAuthenticated) {
                window.location.href = _sign_out;
                return;
            }

            if (!data.IsSuccessful) {
                var retError = data.Error ? data.Error : "Unknown error occured. Please try again later!";
                ErrorToast(retError, "dvError");
                return ShowErrorAlert(retError);
            } else {
                if (modInfo.NextStage === 6) {
                    window.location.href = "/register/app-summary";
                    return false;
                }

                window.location.href = redUrl;
            }

        },
        cache: false,
        complete: function () {
            unblockMainView();
        }
    });
    return false;
}

function processPrev(modInfo) {
    const redUrl = "/register/wizard?progType=" + modInfo.RegType + "&stage=" + modInfo.PrevStage;
    window.location.href = redUrl;
    return false;
}

function processSubmission(modInfo) {
    console.log(modInfo);
}

$("#btnRegNext").on('click', (function () {
    console.log(modInfo);
    if (isEmpty(modInfo)) {
        const sesMsg = "Invalid Session! Please try again"
        ErrorToast(sesMsg, "dvError");
        return ShowErrorAlert(sesMsg);
    }

    const frm = modInfo.FormId;
    $(frm).parsley();
    $(frm).parsley().validate();
    if (!$(frm).parsley().isValid()) {
        ErrorToast(_frmValidError, "dvError");
        return ShowErrorAlert(_frmValidError);
    }
    //Clear Error
    ClearToast("dvError");
    return processNext(modInfo);
}));

$("#btnRegPrev").on('click', (function () {
    return processPrev(modInfo);
}));
$("#btnRegSubmit").on('click', (function () {
    return processSubmission(modInfo);
}));