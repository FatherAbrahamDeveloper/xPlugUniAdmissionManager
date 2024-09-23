function loadDisplayView() {
    jBackupServe.displayListView();
}

function loadDetailView(id) {
    jBackupServe.displayItemView(id);
}

function deleteItemInfo(id) {
    alert("Not Allowed!")
    return false;
    //jBackupServe.deleteItem(id);
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
        return jBackupServe.saveData($(frm), serviceUrl);
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
        return jBackupServe.saveModalData($(frm), serviceUrl);
    });
    return this;
};

jQuery.fn.onSelectionToModal = function (url, type) {
    this.on("click", function () {
        var arr = $('input[name="SelItemIds"]:checked').map(function () {
            return this.value;
        }).get();

        const lnk = url + "?catIds=" + arr.toString() + "&catType=" + type;

        return jBackupServe.openModal(lnk);
    });
    return this;
};

