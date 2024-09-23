function loadDisplayView() {
    jUserServe.displayListView();
}

function loadDetailView(id) {
    jUserServe.displayItemView(id);
}

function deleteItemInfo(id) {
    alert("Not Allowed!")
    return false;
    //jUserServe.deleteItem(id);
}

function switchItemStatus(id, status) {
    jUserServe.switchStatus(id, status);
}

function resetUserPassword(userId) {
    if (userId === "undefined" || userId === "" || parseInt(userId) < 1) {
        return ShowErrorAlert("Invalid Item Selection");
    }
    return ConfirmMessage("Password will be reset! Are you sure?", doPassReset.bind(null, userId))
}

function doPassReset(userId) {
    $.ajax({
        type: "GET",
        url: '/users/reset-user-password?userId=' + parseInt(userId),
        success: function (data) {
            if (!data.IsSuccessful) {
                const errorM = data.Error ? data.Error : "Unknown error occured. Please try again later!";
                InlineErrorMessage(errorM, "dvError");
                return ShowErrorAlert(errorM);
            }
            ShowSuccessAlert(`Password was reset successfully! New Password is: ${data.NewPassword}`);
        },
        complete: function () {
            unblockMainView();
        }
    });
}

jQuery.fn.onSaveData = function (frm, serviceUrl) {
    $(frm).parsley();
    this.on("click", function () {
        //Validation
        $(frm).parsley().validate();
        if (!$(frm).parsley().isValid()) {
            InlineErrorMessage(_frmValidError, "dvError");
            return ShowErrorAlert(_frmValidError);
        }

        //Clear Error
        ClearInlineError("dvError");

        //Save Data
        return jUserServe.saveData($(frm), serviceUrl);
    });
    return this;
};

jQuery.fn.onSavePopupData = function (frm, serviceUrl) {
    $(frm).parsley();
    this.on("click", function () {
        //Validation
        $(frm).parsley().validate();
        if (!$(frm).parsley().isValid()) {
            InlineErrorMessage(_frmValidError, "dvError");
            return ShowErrorAlertInPopup(_frmValidError);
        }

        //Clear Error
        ClearInlineError("dvError");

        //Save Data
        return jUserServe.saveModalData($(frm), serviceUrl);
    });
    return this;
};

jQuery.fn.onSelectionToModal = function (url, type) {
    this.on("click", function () {
        var arr = $('input[name="SelItemIds"]:checked').map(function () {
            return this.value;
        }).get();

        const lnk = url + "?catIds=" + arr.toString() + "&catType=" + type;

        return jUserServe.openModal(lnk);
    });
    return this;
};

