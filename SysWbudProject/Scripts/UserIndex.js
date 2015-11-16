

function populateDeleteUserModal(e) {
    $("#deleteForm").populate($(e).data('id'));
}

function createUser() {

    var Name = $("#Name").val();
    var array = Name.match("^[\\w\\+\\-]+([ ]?[\\w\\-\\+])*$");

    var accessRight = $("#accessRight").val();

    var user = {
        Name: Name,
        AccessRight: accessRight
    }

    $("#NameValidationMsg").text("");
    $("#passwordValidationMsg").text("");
    $("#ValidationMsg").text("");

    if (array == null) {
        $("#NameValidationMsg").text("Incorrect name. Allowed characters : alphanumerical,+,-");
    } else {
        $.ajax({
            type: "POST",
            url: "/User/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(user),
            dataType: "json",
            success: function (result) {

                if (result.error.localeCompare("true") == 0) {
                    $("#ValidationMsg").text("Could not add user to database. Try again later.");
                } else {
                    location.reload(true);
                }

            },
            error: function (result) {
                $("#ValidationMsg").text("Could not add user to database. Try again much later.");
            }
        });



    }


}

function deleteUser() {

    $("#ValidationMsg").text("");

    $.ajax({
        type: "POST",
        url: "/User/Delete",
        contentType: "application/json; charset=utf-8",
        data: '{"id":"' + $("#id").val() + '"}',
        dataType: "json",
        success: function (result) {

            if (result.error.localeCompare("true") == 0) {
                $("#ValidationMsg").text("Could not delete user from database. Try again later.");
            } else {
                location.reload(true);
            }

        },
        error: function (result) {
            $("#ValidationMsg").text("Could not delete user from database. Try again much later.");
        }
    });

}