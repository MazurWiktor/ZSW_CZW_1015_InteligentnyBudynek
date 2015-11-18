function confirmDelete() {

    $("#ValidationMsg").text("");

    $.ajax({
        type: "POST",
        url: "/Building/Delete",
        contentType: "application/json; charset=utf-8",
        data: '{"id":"' + $("#id").val() + '"}',
        dataType: "json",
        success: function (result) {

            if (result.error.localeCompare("true") == 0) {
                $("#ValidationMsg").text("Could not delete building from database. Try again later.");
            } else {
                location.reload(true);
            }

        },
        error: function (result) {
            $("#ValidationMsg").text("Could not delete building from database. Try again much later.");
        }
    });

}

function pop(e) {
    $("#form").populate($(e).data('id'));
}

function create() {

    var Name = $("#Name").val();
    var array = Name.match("^[\\w\\+\\-]+([ ]?[\\w\\-\\+])*$");

    var building = {
        Name: Name
    }

    $("#NameValidationMsg").text("");
    $("#ValidationMsg").text("");

    if (array == null) {
        $("#NameValidationMsg").text("Incorrect name. Allowed characters : alphanumerical,+,-");
    } else {

        $.ajax({
            type: "POST",
            url: "/Building/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(building),
            dataType: "json",
            success: function (result) {

                if (result.error.localeCompare("true") == 0) {
                    $("#ValidationMsg").text("Could not add building to database. Try again later.");
                } else {
                    location.reload(true);
                }

            },
            error: function (result) {
                $("#ValidationMsg").text("Could not add building to database. Try again much later.");
            }
        });



    }


}