class XPAGE_JService {

    #coreItem = {
        servicePath: "",
        saveConfirmAlert: "",
        saveSuccessAlert: "",
        deleteConfirmAlert: "",
        deleteSuccessAlert: "",
    };
    constructor(coreItemObj) {
        this.#coreItem.servicePath = coreItemObj.servicePath;
        this.#coreItem.saveConfirmAlert = `${coreItemObj.itemName} ${_save_confirmation_msg}`;
        this.#coreItem.saveSuccessAlert = `${coreItemObj.itemName} ${_savedSuccessMsg}`;
        this.#coreItem.deleteConfirmAlert = `${coreItemObj.itemName} ${_deleteConfirmMsg}`;
        this.#coreItem.deleteSuccessAlert = `${coreItemObj.itemName} ${_deleteSuccessMsg}`;
    }

    isValid() {
        if (this.#coreItem === null || this.#coreItem.servicePath === "" || this.#coreItem.servicePath.length < 3) return false;
        if (this.#coreItem.saveSuccessAlert === "" || this.#coreItem.saveSuccessAlert.length < 3) this.#coreItem.saveSuccessAlert = _savedSuccessMsg;
        if (this.#coreItem.deleteConfirmAlert === "" || this.#coreItem.deleteConfirmAlert.length < 3) this.#coreItem.deleteConfirmAlert = _deleteConfirmMsg;
        if (this.#coreItem.deleteSuccessAlert === "" || this.#coreItem.deleteSuccessAlert.length < 3) this.#coreItem.deleteSuccessAlert = _deleteSuccessMsg;
        return true;
    }

    displayListView() {
        blockMainView();

        $.when(
            $.ajax({
                url: this.#coreItem.servicePath + "list-view",
                dataType: "html",
                success: function (data) {
                    $('#dvListView').html("");
                    $('#dvListView').html(data);
                }
            })
        ).done(function () {
            unblockMainView();
        });
    }

    displayDashView() {
        blockMainView();

        $.when(
            $.ajax({
                url: this.#coreItem.servicePath + "list",
                dataType: "html",
                success: function (data) {
                    $('#dvDashView').html("");
                    $('#dvDashView').html(data);
                }
            })
        ).done(function () {
            unblockMainView();
        });
    }

    displayItemView(id) {
        if (id === null || id === "" || parseInt(id) < 1) return false;
        blockMainView();
        $.when(
            $.ajax({
                url: this.#coreItem.servicePath + "item-view?itemId=" + parseInt(id),
                dataType: "html",
                success: function (data) {
                    $('#dvItemView').html("");
                    $('#dvItemView').html(data);
                }
            })
        ).done(function () {
            unblockMainView();
        });
    }

    reloadListView(uPath) {
        blockMainView();

        $.when(
            $.ajax({
                url: uPath + "list-view",
                dataType: "html",
                success: function (data) {
                    $('#dvListView').html("");
                    $('#dvListView').html(data);
                }
            })
        ).done(function () {
            unblockMainView();
        });
    }

    deleteItem(id) {
        if (id == "undefined" || id == null || id === "" || parseInt(id) < 1) {
            return ShowErrorAlert("Invalid Item Selection");
        }
        return ConfirmMessage(this.#coreItem.deleteConfirmAlert, this.doDelete.bind(null, id, this.#coreItem))
    }

    doDelete(id, coreItem) {
        const succ = coreItem.deleteSuccessAlert;
        $.ajax({
            type: "POST",
            url: coreItem.servicePath + "delete-item?id=" + parseInt(id),
            success: function (data) {
                unblockMainView();
                if (!data.IsAuthenticated) {
                    window.location.href = _signIn;
                    return;
                }

                if (!data.IsSuccessful) {
                    var retError = data.Error ? data.Error : "Unknown error occured. Please try again later!";
                    ShowErrorAlert(retError);
                } else {
                    ShowSuccessPopupAlertReload(succ);
                }

            },
            cache: false,
            complete: function () {
                unblockMainView();
            }
        });
    }

    switchStatus(id, status) {
        if (id == "undefined" || id == null || id === "" || parseInt(id) < 1) {
            return ShowErrorAlert("Invalid Item Selection");
        }
        const msg = status === 1 ? "Item would be disabled! Are you sure?" : "Item would be enabled! Are you sure?";
        return ConfirmMessage(msg, this.doSwitchStatus.bind(null, id, status, this.#coreItem))
    }

    doSwitchStatus(id, status, coreItem) {
        const succ = status === 1 ? "Item was disabled successfully" : "Item was enabled successfully";
        $.ajax({
            type: "POST",
            url: coreItem.servicePath + `update-item-status?id=${parseInt(id)}&status=${parseInt(status)}`,
            success: function (data) {
                unblockMainView();
                if (!data.IsAuthenticated) {
                    window.location.href = _signIn;
                    return;
                }

                if (!data.IsSuccessful) {
                    var retError = data.Error ? data.Error : "Unknown error occured. Please try again later!";
                    ShowErrorAlert(retError);
                } else {
                    ShowSuccessPopupAlertReload(succ);
                }

            },
            cache: false,
            complete: function () {
                unblockMainView();
            }
        });
    }

    saveData(frm, url) {

        if (frm == null) {

            ErrorToast(_frmValidError, "dvError");
            return ShowErrorAlert(_frmValidError);
        }

        blockMainView();

        const params = frm.serializeArray();
        const succ = this.#coreItem.saveSuccessAlert;
        const servPath = this.#coreItem.servicePath;

        $.ajax({
            type: "POST",
            url: this.#coreItem.servicePath + url,
            data: params,
            success: function (data) {
                unblockMainView();
                if (!data.IsAuthenticated) {
                    window.location.href = _sign_out;
                    return;
                }

                if (!data.IsSuccessful) {
                    var retError = data.Error ? data.Error : "Unknown error occured. Please try again later!";
                    ErrorToast(retError, "dvError");
                    return ShowErrorAlert("Error Occurred! Kindly close this alert window to view details");
                } else {
                    ShowSuccessAlertRedirect(succ, servPath);
                }

            },
            cache: false,
            complete: function () {
                unblockMainView();
            }
        });
        return false;
    }

    saveModalData(frm, url) {

        if (frm == null) {

            ErrorToast(_frmValidError, "dvError");
            return ShowErrorAlertInPopup(_frmValidError);
        }

        blockPopupView();

        const params = frm.serializeArray();
        const succ = this.#coreItem.saveSuccessAlert;
        const thisFunct = this.reloadListView.bind(null, this.#coreItem.servicePath);

        $.ajax({
            type: "POST",
            url: this.#coreItem.servicePath + url,
            data: params,
            success: function (data) {
                //console.log(data);
                unblockPopupView();
                if (!data.IsAuthenticated) {
                    window.location.href = _sign_out;
                    return;
                }

                if (!data.IsSuccessful) {
                    var retError = data.Error ? data.Error : "Unknown error occured. Please try again later!";
                    ErrorToast(retError, "dvError");
                    ShowErrorAlertInPopup("Error Occurred! Kindly close this alert window to view details");
                } else {
                    ShowSuccessPopupAlert(succ, thisFunct);
                }

            },
            cache: false,
            complete: function () {
                unblockPopupView();
            }
        });
        return false;
    }

    saveExtModalData(frm, url) {

        if (frm == null) {

            ErrorToast(_frmValidError, "dvError");
            return ShowErrorAlertInPopup(_frmValidError);
        }

        blockPopupView();

        const params = frm.serializeArray();
        const succ = this.#coreItem.saveSuccessAlert;

        $.ajax({
            type: "POST",
            url: this.#coreItem.servicePath + url,
            data: params,
            success: function (data) {
                unblockPopupView();
                if (!data.IsAuthenticated) {
                    window.location.href = _sign_out;
                    return;
                }

                if (!data.IsSuccessful) {
                    var retError = data.Error ? data.Error : "Unknown error occured. Please try again later!";
                    ErrorToast(retError, "dvError");
                    return ShowErrorAlertInPopup("Error Occurred! Kindly close this alert window to view details");
                } else {
                    return ShowSuccessPopupAlertReload(succ);
                }

            },
            cache: false,
            complete: function () {
                unblockPopupView();
            }
        });
        return false;
    }

    openModal(url) {
        $('#modalContent').load(url, function () {
            $('#dvModal').modal('show', {
                backdrop: 'static',
                keyboard: false,
            });
        });
        return false;
    }

}



