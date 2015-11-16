function clearLogs() {

    $("#ValidationMsg").text("");

    $.ajax({
        type: "POST",
        url: "/Log/Clear",
        contentType: "application/json; charset=utf-8",
        data: '',
        dataType: "json",
        success: function (result) {

            if (result.error.localeCompare("true") == 0) {
                $("#ValidationMsg").text("Could not delete logs from database. Try again later.");
            } else {
                location.reload(true);
            }

        },
        error: function (result) {
            $("#ValidationMsg").text("Could not delete logs from database. Try again much later.");
        }
    });

}