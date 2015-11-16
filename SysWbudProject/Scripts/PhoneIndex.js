

function populateDeletePhoneModal(e) {
    $("#deleteForm").populate($(e).data('id'));
}

function createPhone() {

    var Name = $("#Name").val();
    var array = Name.match("^[\\w\\+\\-]+([ ]?[\\w\\-\\+])*$");

    var macAddress = $("#macAddress").val();

    var userID = $("#userID").val();

    var phoneType = $("#phoneType").val();

    var phone = {
        Name: Name,
        PhoneType: phoneType,
        UserID: userID,
        MacAddress: macAddress
    }

    $("#NameValidationMsg").text("");
    $("#MacAddressValidationMsg").text("");
    $("#ValidationMsg").text("");

    if (array == null) {
        $("#NameValidationMsg").text("Incorrect name. Allowed characters : alphanumerical,+,-");
    } else {
        $.ajax({
            type: "POST",
            url: "/Phone/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(phone),
            dataType: "json",
            success: function (result) {

                if (result.error.localeCompare("true") == 0) {
                    $("#ValidationMsg").text("Could not add phone to database. Try again later.");
                } else {
                    location.reload(true);
                }

            },
            error: function (result) {
                $("#ValidationMsg").text("Could not add phone to database. Try again much later.");
            }
        });



    }


}

function deletePhone() {

    $("#ValidationMsg").text("");

    $.ajax({
        type: "POST",
        url: "/Phone/Delete",
        contentType: "application/json; charset=utf-8",
        data: '{"id":"' + $("#id").val() + '"}',
        dataType: "json",
        success: function (result) {

            if (result.error.localeCompare("true") == 0) {
                $("#ValidationMsg").text("Could not delete phone from database. Try again later.");
            } else {
                location.reload(true);
            }

        },
        error: function (result) {
            $("#ValidationMsg").text("Could not delete phone from database. Try again much later.");
        }
    });

}