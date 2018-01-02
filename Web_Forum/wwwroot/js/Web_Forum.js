$("#createUserForm button").click(function () {

    $.ajax({
        url: '/user/',
        method: 'POST',
        data: {
            "UserName": $("#createUserForm [name=CreateUserName]").val(),
            "Email": $("#createUserForm [name=CreateUserEmail]").val(),
            "Password": $("#createUserForm [name=CreateUserPassword]").val()
        }

    })
        .done(function (result) {
            alert("Du har skapat användaren ");
       
            console.log("Success!", result)

        })

        .fail(function (xhr, status, error) {

            alert("fail");
            console.log("Error", xhr, status, error);

        })

});