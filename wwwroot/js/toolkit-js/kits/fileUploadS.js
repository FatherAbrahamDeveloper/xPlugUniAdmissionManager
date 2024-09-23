$(function () {
    $("#frmAdd").on("submit", function (e) {
        e.preventDefault();

        console.log('Doing ajax submit');

        var formAction = $(this).attr("action");
        var fdata = new FormData();

        var fileInput = $('#uploadfile')[0];
        var file = fileInput.files[0];
        fdata.append("file", file);

        $.ajax({
            type: 'post',
            url: formAction,
            data: fdata,
            processData: false,
            contentType: false
        }).done(function (result) {
            // do something with the result now
            console.log(result);
            if (result.status === "success") {
                alert(result.url);
            } else {
                alert(result.message);
            }
        });
    });
})

$(document).on("ready", function () {
    //Dropdownlist Selectedchange event  
    $("#Country").on("change", function () {
        $("#State").empty();
        $.ajax({
            type: 'POST',
            url: '@Url.Action("GetStates")', // we are calling json method  

            dataType: 'json',

            data: { id: $("#Country").val() },
            // here we are get value of selected country and passing same value    as inputto json method GetStates.
            success: function (states) {
                // states contains the JSON formatted list  
                // of states passed from the controller  

                $.each(states, function (i, state) {
                    $("#State").append('<option value="' + state.Value + '">' +
                        state.Text + '</option>');
                    // here we are adding option for States  

                });
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })
});

$(document).on("ready", function () {
    //Dropdownlist Selectedchange event  
    $("#State").on("change", function () {
        $("#city").empty();
        $.ajax({
            type: 'POST',
            url: '@Url.Action("GetCity")',
            dataType: 'json',
            data: { id: $("#State").val() },
            success: function (citys) {
                // states contains the JSON formatted list  
                // of states passed from the controller  
                $.each(citys, function (i, city) {
                    $("#city").append('<option value="'
                        + city.Value + '">'
                        + city.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })
});