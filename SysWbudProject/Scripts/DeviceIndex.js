

function populateDeleteDeviceModal(e) {
    $("#deleteForm").populate($(e).data('id'));
}

function createDevice() {

    var Name = $("#Name").val();
    var array = Name.match("^[\\w\\+\\-]+([ ]?[\\w\\-\\+])*$");

    var deviceType = $("#deviceType").val();

    var hardwareId = $("#hardwareId").val();

    var roomID = $("#roomID").val();

    var device = {
        Name: Name,
        DeviceType: deviceType,
        RoomID: roomID,
        hardwareID : hardwareId
    }

    $("#NameValidationMsg").text("");
    $("#ValidationMsg").text("");

    if (array == null) {
        $("#NameValidationMsg").text("Incorrect name. Allowed characters : alphanumerical,+,-");
    } else {

        $.ajax({
            type: "POST",
            url: "/Device/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(device),
            dataType: "json",
            success: function (result) {

                if (result.error.localeCompare("true") == 0) {
                    $("#ValidationMsg").text("Could not add device to database. Try again later.");
                } else {
                    location.reload(true);
                }

            },
            error: function (result) {
                $("#ValidationMsg").text("Could not add device to database. Try again much later.");
            }
        });



    }


}

function deleteDevice() {

    $("#ValidationMsg").text("");

    $.ajax({
        type: "POST",
        url: "/Device/Delete",
        contentType: "application/json; charset=utf-8",
        data: '{"id":"' + $("#id").val() + '"}',
        dataType: "json",
        success: function (result) {

            if (result.error.localeCompare("true") == 0) {
                $("#ValidationMsg").text("Could not delete device from database. Try again later.");
            } else {
                location.reload(true);
            }

        },
        error: function (result) {
            $("#ValidationMsg").text("Could not delete device from database. Try again much later.");
        }
    });

}