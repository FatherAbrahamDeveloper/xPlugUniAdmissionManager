jQuery.fn.onSearchLogs = function (frm) {

    this.on("click", function () {
        //Validation
        $(frm).parsley().validate();
        if (!$(frm).parsley().isValid()) {
            InlineErrorMessage("Invalid Search Parameter", "dvError");
            return ShowErrorAlert("Invalid Search Parameter");
        }

        //Clear Error
        ClearInlineError("dvError");

        var param = {
            "SelectedId": $("#BackupItemd").val(),
            "SelDate": $("#txtDate").val()
        };

        blockMainView();

        $.ajax({
            type: "POST",
            url: '/backup-reports/list-view',
            data: param,
            success: function (data) {
                $("#dvListView").html("");
                $("#dvListView").html(data);
            },
            complete: function () {
                unblockMainView();
            }
        });

        ''
    });
    return this;
};