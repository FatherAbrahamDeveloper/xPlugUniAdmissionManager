function loadDisplayView() {
    jDatabaseServe.displayListView();
}

function loadDetailView(id) {
    jDatabaseServe.displayItemView(id);
}

function deleteItemInfo(id) {
    alert("Not Allowed!")
    return false;
    //jDatabaseServe.deleteItem(id);
}

function switchItemStatus(id, status) {
    jDatabaseServe.switchStatus(id, status);
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
        return jDatabaseServe.saveData($(frm), serviceUrl);
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
        return jDatabaseServe.saveModalData($(frm), serviceUrl);
    });
    return this;
};

jQuery.fn.onSelectionToModal = function (url, type) {
    this.on("click", function () {
        var arr = $('input[name="SelItemIds"]:checked').map(function () {
            return this.value;
        }).get();

        const lnk = url + "?catIds=" + arr.toString() + "&catType=" + type;

        return jDatabaseServe.openModal(lnk);
    });
    return this;
};

