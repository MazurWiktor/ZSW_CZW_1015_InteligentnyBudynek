

function populateDeleteRoomModal(e) {
    $("#deleteForm").populate($(e).data('id'));
}

function createRoom() {

    var Name = $("#Name").val();
    var array = Name.match("^[\\w\\+\\-]+([ ]?[\\w\\-\\+])*$");

    var FloorID = $("#FloorID").val();

    var room = {
        Name: Name,
        FloorID: FloorID
    }

    $("#NameValidationMsg").text("");
    $("#ValidationMsg").text("");

    if (array == null) {
        $("#NameValidationMsg").text("Incorrect name. Allowed characters : alphanumerical,+,-");
    } else {

        $.ajax({
            type: "POST",
            url: "/Room/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(room),
            dataType: "json",
            success: function (result) {

                if (result.error.localeCompare("true") == 0) {
                    $("#ValidationMsg").text("Could not add room to database. Try again later.");
                } else {
                    location.reload(true);
                }

            },
            error: function (result) {
                $("#ValidationMsg").text("Could not add room to database. Try again much later.");
            }
        });



    }


}

function deleteRoom() {

    $("#ValidationMsg").text("");

    $.ajax({
        type: "POST",
        url: "/Room/Delete",
        contentType: "application/json; charset=utf-8",
        data: '{"id":"' + $("#id").val() + '"}',
        dataType: "json",
        success: function (result) {

            if (result.error.localeCompare("true") == 0) {
                $("#ValidationMsg").text("Could not delete room from database. Try again later.");
            } else {
                location.reload(true);
            }

        },
        error: function (result) {
            $("#ValidationMsg").text("Could not delete room from database. Try again much later.");
        }
    });

}