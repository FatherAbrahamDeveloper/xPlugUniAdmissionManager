function loadDisplayView() {
    jCourseTypeServe.displayListView();
}

function loadDetailView(id) {
    jCourseTypeServe.displayItemView(id);
}

function deleteItemInfo(id) {
    alert("Not Allowed!")
    return false;
    //jCourseTypeServe.deleteItem(id);
}

function switchItemStatus(id, status) {
    jCourseTypeServe.switchStatus(id, status);
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
        return jCourseTypeServe.saveData($(frm), serviceUrl);
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
        return jCourseTypeServe.saveModalData($(frm), serviceUrl);
    });
    return this;
};

jQuery.fn.onSelectionToModal = function (url, type) {
    this.on("click", function () {
        var arr = $('input[name="SelItemIds"]:checked').map(function () {
            return this.value;
        }).get();

        const lnk = url + "?catIds=" + arr.toString() + "&catType=" + type;

        return jCourseTypeServe.openModal(lnk);
    });
    return this;
};

