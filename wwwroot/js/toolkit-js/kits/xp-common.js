
function XP_InitDatePicker(id, startDate, stopDate, dayDiff, showDropDown) {

    let cntId = '#' + id;
    let dDiff = 0;
    let dropDown = false;

    //Do Default Dates
    if (dayDiff !== null && dayDiff !== "" && parseInt(dayDiff) > -1) {
        dDiff = parseInt(dayDiff);
    }

    if (showDropDown !== null && showDropDown !== "") {
        dropDown = showDropDown;
    }

    //Set Defaults
    let start = moment();
    let end = moment();

    //Check passed dates
    if (startDate !== null && startDate !== "" && startDate.length == 10) {
        start = moment(startDate, 'DD-MM-YYYY');
    }
    else {
        start = moment().subtract(dDiff, 'days');
    }

    if (stopDate !== null && stopDate !== "" && stopDate.length == 10) {
        end = moment(stopDate, 'DD-MM-YYYY');
    }


    function cb(start, end) {
        $(cntId + ' span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY')); //html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    }

    //Config Picker
    const pickerConfig = {
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    };

    if (dDiff > 0) {
        pickerConfig.maxSpan = { "days": dDiff };
    }

    if (dropDown) {
        pickerConfig.showDropdowns = true;
    }

    $(cntId).daterangepicker(pickerConfig, cb);
    cb(start, end);

};


//====================== Check

function isEmpty(value) {
    return (
        // null or undefined
        (value == null) ||

        // has length and it's zero
        (value.hasOwnProperty('length') && value.length === 0) ||

        // is an Object and has no keys
        (value.constructor === Object && Object.keys(value).length === 0)
    )
}

//====================== Message Toasts
function ErrorToast(msg, id) {
    if (id == undefined) {
        alert(msg);
    } else {
        var html = '<div class="alert alert-danger "><a href="#" class="close" data-dismiss="alert" aria-label="close">×</a><i class="fa fa-remove fa-2x"></i>' + msg + '</div>';
        $("#" + id).html(html);
    }
}
function SuccessToast(msg, id) {
    if (id == undefined) {
        alert(msg);
    } else {
        var html = '<div class="alert alert-success "><a href="#" class="close" data-dismiss="alert" aria-label="close">×</a> <i class="fa fa-check fa-2x"></i><strong> Success!</strong> ' + msg + '</div>';
        $("#" + id).html(html);
    }
}

function ClearToast(id) {
    $("#" + id).html("");
}


//============================= Sweet Alert
/*
Swal.fire({
  title: "<strong>HTML <u>example</u></strong>",
  icon: "info",
  html: `
    You can use <b>bold text</b>,
    <a href="#" autofocus>links</a>,
    and other HTML tags
  `,
  showCloseButton: true,
  showCancelButton: true,
  focusConfirm: false,
  confirmButtonText: `
    <i class="fa fa-thumbs-up"></i> Great!
  `,
  confirmButtonAriaLabel: "Thumbs up, great!",
  cancelButtonText: `
    <i class="fa fa-thumbs-down"></i>
  `,
  cancelButtonAriaLabel: "Thumbs down"
});
*/
function ShowErrorAlert(msg) {

    Swal.fire({
        icon: "error",
        title: "Error Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Close",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            unblockMainView();
        }

    });

    return false;
}
function ShowErrorAlertInPopup(msg) {
    Swal.fire({
        icon: "error",
        title: "Error Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Close",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            unblockPopupView();
        }
    });

    return false;
}
function ConfirmMessage(msg, callback) {
    Swal.fire({
        title: "Confirmation",
        text: msg,
        icon: "warning",
        showDenyButton: true,
        confirmButtonColor: "#3085d6",
        denyButtonColor: "#d33",
        confirmButtonText: "Yes",
        denyButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            if (callback && typeof callback === "function") {
                blockMainView();
                setTimeout(function () {
                    return callback();
                }, 200);
            }
        }
    });

}

function ConfirmOnPopup(msg, callback) {
    Swal.fire({
        title: "Confirmation",
        text: msg,
        icon: "warning",
        showDenyButton: true,
        confirmButtonColor: "#3085d6",
        denyButtonColor: "#d33",
        confirmButtonText: "Yes",
        denyButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            if (callback && typeof callback === "function") {
                setTimeout(function () {
                    return callback();
                }, 200);
            }
        }
    });
}

function ConfirmRedirect(msg, url) {
    Swal.fire({
        title: "Confirmation",
        text: msg,
        icon: "warning",
        showDenyButton: true,
        confirmButtonColor: "#3085d6",
        denyButtonColor: "#d33",
        confirmButtonText: "Yes",
        denyButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            if (url && url.length > 3) {
                window.location.href = url;
            }
        }
    });

}
function ShowSuccessAlert(msg, callback) {
    Swal.fire({
        icon: "success",
        title: "Success Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            if (callback && typeof callback === "function") {
                setTimeout(function () {
                    return callback();
                }, 200);
            }
        }
    });
}
function ShowSuccessAlertRedirect(msg, url) {
    Swal.fire({
        icon: "success",
        title: "Success Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = url;
        }
    });
}
function ShowSuccessAlertReload(msg) {
    Swal.fire({
        icon: "success",
        title: "Success Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.reload();
        }
    });
}
function ShowSuccessPopupAlert(msg, callback) {
    Swal.fire({
        icon: "success",
        title: "Success Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $('#dvModal').modal('hide');
            if (callback && typeof callback === "function") {
                setTimeout(function () {
                    callback();
                }, 200);
            }
        }
    });
}
function ShowSuccessPopupAlertReload(msg) {
    Swal.fire({
        icon: "success",
        title: "Success Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $('#dvModal').modal('hide');
            window.location.reload();
        }
    });

}

function ShowSuccessPopupAlertRedirect(msg, url) {
    Swal.fire({
        icon: "success",
        title: "Success Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $('#dvModal').modal('hide');
            window.location.href = url;
        }
    });

}

function ShowSuccessPopupAlertClose(msg) {
    Swal.fire({
        icon: "success",
        title: "Success Alert",
        text: msg,
        showCancelButton: false,
        confirmButtonText: "Ok",
        closeOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $('#dvModal').modal('hide');
        }
    });
}

//callbackTester(tryMe.bind(null, "hello", "goodbye"));
//callbackTester(function () {
//    tryMe("hello", "goodbye");
//});
//function callbackTester(callback) {
//    callback(arguments[1], arguments[2]);
//}
//callbackTester(tryMe, "hello", "goodbye");