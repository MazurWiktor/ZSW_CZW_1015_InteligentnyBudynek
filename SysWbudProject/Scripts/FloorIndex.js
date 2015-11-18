

function populateDeleteFloorModal(e) {
    $("#deleteForm").populate($(e).data('id'));
}

function createFloor() {

    var Name = $("#Name").val();
    var array = Name.match("^[\\w\\+\\-]+([ ]?[\\w\\-\\+])*$");

    var BuildingID = $("#BuildingID").val();

    var floor = {
        Name: Name,
        BuildingID: BuildingID
    }

    $("#NameValidationMsg").text("");
    $("#ValidationMsg").text("");

    if (array == null) {
        $("#NameValidationMsg").text("Incorrect name. Allowed characters : alphanumerical,+,-");
    } else {

        $.ajax({
            type: "POST",
            url: "/Floor/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(floor),
            dataType: "json",
            success: function (result) {

                if (result.error.localeCompare("true") == 0) {
                    $("#ValidationMsg").text("Could not add floor to database. Try again later.");
                } else {
                    location.reload(true);
                }

            },
            error: function (result) {
                $("#ValidationMsg").text("Could not add floor to database. Try again much later.");
            }
        });



    }


}

function deleteFloor() {

    $("#ValidationMsg").text("");

    $.ajax({
        type: "POST",
        url: "/Floor/Delete",
        contentType: "application/json; charset=utf-8",
        data: '{"id":"' + $("#id").val() + '"}',
        dataType: "json",
        success: function (result) {

            if (result.error.localeCompare("true") == 0) {
                $("#ValidationMsg").text("Could not delete floor from database. Try again later.");
            } else {
                location.reload(true);
            }

        },
        error: function (result) {
            $("#ValidationMsg").text("Could not delete floor from database. Try again much later.");
        }
    });

}