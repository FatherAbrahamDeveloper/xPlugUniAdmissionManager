
function changePass() {
    $('#frmChangePassword').parsley().on('field:validated', function () {
        var ok = $('.parsley-error').length === 0;
        if (!ok) {
            InlineErrorMessage("Validation Error Occurred!", "dvError");
            return false;
        }
    });

    ClearInlineError("dvError");

    var oldPassword = $("#CurrentPassword").val();
    var newPassword = $("#NewPassword").val();
    var confirmPassword = $("#ConfirmNewPassword").val();
    var username = $("#Username").val();
    var userIdVal = $("#UserId").val();

    if (username == null || username === "" || username.length < 3) {
        InlineErrorMessage("You Session Has Expired", "dvError");
        return false;
    }

    if (userIdVal == null || userIdVal === "") {
        InlineErrorMessage("You Session Has Expired", "dvError");
        return false;
    }

    if (parseInt(userIdVal) < 1) {
        InlineErrorMessage("You Session Has Expired", "dvError");
        return false;
    }

    if (oldPassword == null || oldPassword === "" || oldPassword.length < 4) {
        InlineErrorMessage("Current Password must be at least 4 characters", "dvError");
        return false;
    }

    if (newPassword == null || newPassword === "" || newPassword.length < 4) {
        InlineErrorMessage("New Password must be at least 4 characters", "dvError");
        return false;
    }

    if (confirmPassword == null || confirmPassword === "" || confirmPassword.length < 4) {
        InlineErrorMessage("Confirm Password must be at least 4 characters", "dvError");
        return false;
    }

    if (newPassword == oldPassword) {
        InlineErrorMessage("Current and New Password must be different", "dvError");
        return false;
    }

    if (newPassword != confirmPassword) {
        InlineErrorMessage("New Password and Confirm Password must be equal", "dvError");
        return false;
    }

    var old_pass_crypt = encyItem(oldPassword);
    var new_pass_crypt = encyItem(newPassword);
    var confirm_pass_crypt = encyItem(confirmPassword);
    var uname_crypt = encyItem(username);

    if (old_pass_crypt == "" || old_pass_crypt.length < 5 || new_pass_crypt == "" || new_pass_crypt.length < 5 || confirm_pass_crypt == "" || confirm_pass_crypt.length < 5
        || uname_crypt == "" || uname_crypt.length < 5) {
        InlineErrorMessage("System Security Error! Please try again later", "dvError");
        return false;
    }

    $('#popupViewDiv').block({
        css: {
            border: 'none', padding: '10px',
            backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: .5, color: '#fff'
        }
    });

    var param = {};
    param.CurrentPassword = old_pass_crypt;
    param.NewPassword = new_pass_crypt;
    param.ConfirmNewPassword = new_pass_crypt;
    param.UserId = parseInt(userIdVal);
    param.Username = uname_crypt;

    $.ajax({
        type: "POST",
        url: '/account/change-password',
        data: param,
        dataType: "json",
        success: function (retVal) {
            if (!retVal.IsAuthenticated) {
                alert('Not Auth');
                window.location.href = '/account/sign-out';
                return;
            }
            if (!retVal.IsSuccessful) {
                InlineErrorMessage(retVal.Error ? retVal.Error : "Unknown error occured. Please try again later!", "dvError");
                swal({
                    title: "",
                    text: retVal.Error ? retVal.Error : "Unknown error occured. Please try again later!",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonText: "Close",
                    closeOnConfirm: true,
                }, function (isConfirm) {
                    if (isConfirm) { }
                });

            } else {
                swal({
                    title: "",
                    text: "Password was changed successfully",
                    type: "success",
                    showCancelButton: false,
                    confirmButtonText: "Ok",
                    closeOnConfirm: true,
                }, function (isConfirm) {
                    if (isConfirm) {
                        window.location.reload();
                    }
                });
            }
        },
        complete: function () {
            $('#popupViewDiv').unblock();
        }
    });
    return false;
}

