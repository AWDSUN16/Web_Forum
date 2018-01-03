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

$("#userLoginForm button").click(function () {

    $.ajax({
        url: '/user/login',
        method: 'POST',
        data: {
            "email": $("#userLoginForm [name=UserLoginEmail]").val()
        }

    })
        .done(function (result) {
                alert("Användare " + result.userName + " har loggats in!");

                console.log("Success!", result)

        })

        .fail(function (xhr, status, error) {

                alert("fail");
                console.log("Error", xhr, status, error);

        })

});

$("#userLogOut button").click(function () {

    $.ajax({
        url: '/user/logout',
        method: 'POST'
    })
        .done(function () {
            alert("Användaren har loggats ut!");

            console.log("Success!")

        })
        .fail(function (xhr, status, error) {

            alert("fail!");
            console.log("Error", xhr, status, error);

        })

});