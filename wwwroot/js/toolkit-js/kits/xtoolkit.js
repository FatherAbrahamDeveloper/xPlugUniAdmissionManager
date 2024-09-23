
jQuery.fn.ForceNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                return (
                    key === 8 ||
                    key === 9 ||
                    key === 13 ||
                    /* key === 46 ||*/
                    key === 110 ||
                    /*key === 190 ||*/
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };

jQuery.fn.ForceDecimal =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                return (
                    key === 8 ||
                    key === 9 ||
                    key === 13 ||
                    key === 46 ||
                    key === 110 ||
                    key === 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };

jQuery.fn.ForceRichText =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                return (
                    key === 8 ||
                    key === 9 ||
                    key === 13 ||
                    key === 16 ||
                    key === 32 ||
                    key === 20 ||
                    key === 46 ||
                    key === 110 ||
                    key === 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 65 && key <= 90) ||
                    (key >= 96 && key <= 111) ||
                    (key >= 186 && key <= 191));
            });
        });
    };

jQuery.fn.ForcePlainText =
    function () {
        return this.on('input', function () {
            var c = this.selectionStart,
                r = /[^a-z0-9-_ ]/gi, //A-Z
                v = $(this).val();
            if (r.test(v)) {
                $(this).val(v.replace(r, ''));
                c--;
            }
            this.setSelectionRange(c, c);
        });
    };

jQuery.fn.ForceSpecialText =
    function () {
        return this.on('input', function () {
            var c = this.selectionStart,
                r = /[^a-z0-9-_()%*&$. ]/gi, //A-Z
                v = $(this).val();
            if (r.test(v)) {
                $(this).val(v.replace(r, ''));
                c--;
            }
            this.setSelectionRange(c, c);
        });
    };
var blockGlobalView = function () {
    $('#dvGlobalView').block({
        css: {
            border: 'none', padding: '10px',
            backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: .5, color: '#fff'
        }
    });
};
var unblockGlobalView = function () {
    $('#dvGlobalView').unblock();
};

var blockMainView = function () {
    $('#mainViewDiv').block({
        css: {
            border: 'none', padding: '10px',
            backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: .5, color: '#fff'
        }
    });
};
var unblockMainView = function () {
    $('#mainViewDiv').unblock();
};

var blockSideView = function (sType) {
    const vType = sType === "" ? "primary" : sType;
    $('#sideViewDiv').block({
        message: '<div class="spinner-border text-' + vType + '" role="status"><span class="visually-hidden"></span></div>',
        css: {
            border: 'none', padding: '30px',
            backgroundColor: '#000', '-webkit-border-radius': '30px', '-moz-border-radius': '30px', opacity: .5, color: '#fff'
        }
    });
};
var unblockSideView = function () {
    $('#sideViewDiv').unblock();
};

var blockSideViewPlain = function (sType) {
    const vType = sType === "" ? "primary" : sType;
    $.blockUI.defaults.overlayCSS = {};
    $('#sideViewDiv').block({
        message: '<div class="spinner-border text-' + vType + '" role="status"><span class="visually-hidden"></span></div>',
        css: {
            border: 'none', backgroundColor: 'none', padding: 'none',
        },
        blockOverlay: { backgroundcolor: 'none' }
    });
};

var blockPopupView = function () {
    $('#popupViewDiv').block({
        css: {
            border: 'none', padding: '10px',
            backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: .5, color: '#fff'
        }
    });
};

var unblockPopupView = function () {
    $('#popupViewDiv').unblock();
};


var blockinlineView = function () {
    $.blockUI.defaults.overlayCSS = {};
    $('#inlineViewDiv').block({
        message: '<img src="/_content/SDMSWeb.UIComponent/sdms-im/common/loading-2-sm.gif" />',
        css: {
            border: 'none', backgroundColor: 'none', padding: 'none'
        },
        blockOverlay: { backgroundcolor: 'none' }
    });
};
var unblockinlineView = function () {
    $('#inlineViewDiv').unblock();
};
var blockinlineProgress = function (sType) {
    const vType = sType === "" ? "primary" : sType;
    $.blockUI.defaults.overlayCSS = {};
    $('#inlineProgressDiv').block({
        message: '<div class="spinner-border text-' + vType + '" role="status"><span class="visually-hidden"></span></div>',
        css: {
            border: 'none', backgroundColor: 'none', padding: 'none'
        },
        blockOverlay: { backgroundcolor: 'none' }
    });
};
var unblockinlineProgress = function () {
    $('#inlineProgressDiv').unblock();
};

function ErrorMessage(msg, id) {
    sweetAlert("", msg, "error");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}

function SuccessMessage(msg, id) {
    sweetAlert("", msg, "success");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}
function InfoMessage(msg, id) {
    sweetAlert("", msg, "info");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}


function fix_chars2(textBox) {
    //textBox.value = textBox.value.replace(/[@'&_,%`"*#|<>;]/g, "");
    var strVal;
    var strVal1;
    var strVal2;
    var dot;
    var i;
    var strComma;
    strVal2 = "";
    strComma = "";
    strVal1 = "";
    strVal = textBox.value;
    dot = 0;
    for (i = 0; i < strVal.length; i++) {
        if (strVal.substring(i, i + 1).indexOf('.') > -1) {
            dot = dot + 1;
        }
        if ((strVal.substring(i, i + 1).indexOf('0') > -1) || (strVal.substring(i, i + 1).indexOf("1") > -1) || (strVal.substring(i, i + 1).indexOf("2") > -1) || (strVal.substring(i, i + 1).indexOf("3") > -1) || (strVal.substring(i, i + 1).indexOf("4") > -1) || (strVal.substring(i, i + 1).indexOf("5") > -1) || (strVal.substring(i, i + 1).indexOf("6") > -1) || (strVal.substring(i, i + 1).indexOf("7") > -1) || (strVal.substring(i, i + 1).indexOf("8") > -1) || (strVal.substring(i, i + 1).indexOf("9") > -1) || ((strVal.substring(i, i + 1).indexOf('.') > -1) && dot <= 1)) {
            strVal1 = strVal1 + strVal.substring(i, i + 1);
        }
    }
    if ((strVal1.indexOf('.') == 0)) {
        strVal1 = "0" + strVal1;
    }
    if (strVal1.indexOf('.') > 0) {
        if (((strVal1.length) - (strVal1.indexOf('.') + 1)) > 2) {
            strVal1 = strVal1.substring(0, strVal1.indexOf('.') + 3);
        }
    }

    strVal = "";
    if (strVal1.indexOf('.') != -1) {

        strVal = strVal1.substring(strVal1.indexOf('.'), strVal1.indexOf('.') + 3);
        strVal1 = strVal1.substring(0, strVal1.indexOf('.'));
    }
    //	    	        alert(strVal1.indexOf('.'));

    while (strVal1.length > 0) {
        if (strVal1.length > 3) {
            strVal2 = strVal1.substring(strVal1.length - 3, strVal1.length) + strComma + strVal2;
            strVal1 = strVal1.substring(0, strVal1.length - 3);
            strComma = ",";
        }
        else {
            strVal2 = strVal1 + strComma + strVal2;
            strVal1 = "";
        }
    }

    //	    	    if (strVal.length>0){
    //	    	    strVal= strVal;
    //	    	    }

    if (strVal2.indexOf('.') > 0) {
        strVal2 = strVal2.substring(0, strVal2.indexOf('.'));
        alert(strVal2);
    }

    textBox.value = strVal2 + strVal;
}