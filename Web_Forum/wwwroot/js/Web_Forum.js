$(document).ready(function () {
    updateNavBar();
    getAllThreads();
});
//test
function updateNavBar() {
    $.ajax({
        url: '/user/checkIfUserIsAuthenticated',
        method: 'GET'
    })
        .done(function (result) {
            { $("#userNameGoesHere").html("<span>"+result+"</span>"); }
            if (result == "admin")
            { $("#showUsernameWhenSingedIn").show();}
           
        })

        .fail(function (xhr, status, error) {

            alert("fail");
            console.log("Error", xhr, status, error);

        });

}

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
            "username": $("#userLoginForm [name=signInUserName]").val(),
            "password": $("#userLoginForm [name=signInPassword]").val()
        }

    })
        .done(function (result) {
            updateNavBar();
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
        .done(function (result) {
            updateNavBar();
            alert("Användare " + result + " Har loggats ut");
            console.log("Success!")

        })
        .fail(function (xhr, status, error) {

            alert("fail");
            console.log("Error", xhr, status, error);

        })

});

$("#createPostForm button").click(function () {

    $.ajax({
        url: '/user/post',
        method: 'POST',
        data: {
            "Content": $("#createPostForm [name=CreatePost]").val()

        }

    })
        .done(function (result) {
            alert("Användaren " + result + " har skapat ett inlägg");

            console.log("Success!", result)

        })

        .fail(function (xhr, status, error) {

            alert("fail");
            console.log("Error", xhr, status, error);

        })

});

$("#showTestPostsButton button").click(function () {

    $.ajax({
        url: '/user/showallposts',
        method: 'GET',
        data: {

        }

    })
        .done(function (result) {
            var list = ''
            for (i = 0; i < result.length; i++) {
                list += "<p style='border:3px; border-style:solid; border-color:#FF0000; padding:1em;' > " + result[i].content + " Skapad av: " + result[i].createdBy + " Klockan " + result[i].dateOfCreation + "<p>" + '<br>';
            };
            $('#showTestPosts').html(list);

            console.log("Success!", result)

        })

        .fail(function (xhr, status, error) {

            alert("fail");
            console.log("Error", xhr, status, error);

        })

});


$("#adminCreate button").click(function () {

    $.ajax({
        url: '/user/createadmin',
        method: 'POST',
        data: {


        }

    })
        .done(function () {

            alert("du har skapat en admin")
            console.log("Success!")

        })

        .fail(function (xhr, status, error) {

            alert("fail");
            console.log("Error", xhr, status, error);

        })

});


$("#showUsernameWhenSingedIn  button").click(function () {

    $.ajax({
        url: '/user/showalluseradminspecific',
        method: 'GET',
        data: {


        }

    })
        .done(function (result) {
            var list = ''
            for (i = 0; i < result.length; i++) {
                list += "Användarnamn: " + result[i].userName+" Email: "+ result[i].email + "<button class='deleteButton' data-id='" + result[i].id + "'>ta bort</button>"+"<br>";
            };
            $('#showTestPosts').html(list);
            
            console.log(status);
        })
        .fail(function (xhr, status, error) {
            alert("fail");
            console.log("Error", xhr, status, error);
        })
});

$("body").on("click", ".deleteButton", function () {

    let clickedId = $(this).data("id")
    console.log(clickedId)
    $.ajax({
        url: '/user/delete',
        method: 'DELETE',
        data: {
            clickedId
        }

    })
        .done(function (result) {
            alert(result)
            console.log(status);

        })

        .fail(function (xhr, status, error) {

            alert(`Fail!`)
            console.log("Error", xhr, status, error);

        })
});

$("#createThreadAndPostForm button").click(function () {

    $.ajax({
        url: '/contents/',
        method: 'POST',
        data: {
            "Title": $("#createThreadAndPostForm [name=CreateThreadTitle]").val(),
            "Content": $("#createThreadAndPostForm [name=CreatePostContent]").val()
        }
    })
        .done(function (result) {
            getAllThreads();
            console.log(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");
            console.log("Error", xhr, status, error)
        });
});


function getAllThreads() {

    $.ajax({
        url: '/contents/getAllThreads',
        method: 'GET'
    })
        .done(function (result) {
            $('#threadDiv').html(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");
            console.log("Error", xhr, status, error)
        });

}



// 1. WHEN YOU CLICK A THREAD-LINK
$(document).on("click", "a.threadLink", function () {
    // Gets the id from the currently clicked <a>-tag's "thread-id""
    var currentlyClickedThreadId = $(this).attr("thread-id");
    // Sets the "thread-id" of the <div id="threadDataDiv">-tag to that of the currently ClickedThreadId variable.
    $("#threadDataDiv").attr("thread-id", currentlyClickedThreadId);

    $.ajax({
        url: '/contents/threads/',
        method: 'GET',
        data: {
            "id": currentlyClickedThreadId
        }
    })
        .done(function (result) {
            fillTableWithThreadPosts(result.id);
            console.log(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");
            console.log("Error", xhr, status, error)
        });

});

//2. WHEN THE THREAD RETURNS ITS POSTS - id referes to the Id of the Thread sent earlier
function fillTableWithThreadPosts(id) {
    // Clean the div of all posts
    $("#threadDataDiv tr").empty();
    // Set a variable according to the threadDataDiv's "thread-id".
    var threadId = $("#threadDataDiv").attr("thread-id");

    $.ajax({
        url: '/contents/threads/' + id + '/posts',
        method: 'GET',
        data: {
            "id": id
        }
    })
        .done(function (result) {
            $("#threadDataDiv").html(result);
            buildThreadPostForm();
            console.log(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");

            console.log("Error", xhr, status, error)
        });

}

//3. AFTER THE POSTS HAVE BEEN RETURNED, A POSTFORM IS BUILT
function buildThreadPostForm() {
    // Set a variable according to the threadDataDiv's "thread-id".
    var threadId = $("#threadDataDiv").attr("thread-id");
    var html = '<div id="threadPostForm" data-id="' + threadId + '">';
    html += '<textarea name="CreatePostContent" placeholder="Skriv ett inlägg..." ></textarea>';
    html += '<button class="sendThreadForm">Svara</button></td>';
    html += '</div >';

    $("#threadDataDiv").append(html);
}

function emptythreadDataDiv() {
    $("#threadDataDiv").empty();
}

$(document).on("click", "button.sendThreadForm", function () {
    //Get the thread.Id from the thread-id attribute inside the post-form
    var id = $("#threadPostForm").data('id');

    $.ajax({
        url: '/contents/threads/' + id + '/posts',
        method: 'POST',
        data: {
            "id": id,
            "Content": $("#threadPostForm [name=CreatePostContent]").val()  
        }
    })
        .done(function (result) {
            console.log(result);

            // Fill the table with new posts after one has been made
            var threadId = result.threadId;
            fillTableWithThreadPosts(threadId);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");
        });

});

$("body").on("click", ".adminthreaddeleteButton", function () {

    let clickedId = $(this).data("id")
    console.log(clickedId)
    $.ajax({
        url: '/contents/adminthreaddelete',
        method: 'DELETE',
        data: {
            clickedId
        }

    })
        .done(function (result) {
            alert(result)
            console.log(status);

        })

        .fail(function (xhr, status, error) {

            alert(`Fail!`)
            console.log("Error", xhr, status, error);

        })
});

$("body").on("click", ".userdeletepost", function () {

    let clickedId = $(this).data("id")
    console.log(clickedId)
    $.ajax({
        url: '/contents/deletepost',
        method: 'DELETE',
        data: {
            clickedId
        }

    })
        .done(function (result) {
            alert(result)
            console.log(status);

        })

        .fail(function (xhr, status, error) {

            alert(`Fail!`)
            console.log("Error", xhr, status, error);

        })
});

$("body").on("click", ".usereditpost", function () {

    let clickedId = $(this).data("id")
    console.log(clickedId)
    $.ajax({
        url: '/contents/editpost',
        method: 'PUT',
        data: {
            clickedId
        }

    })
        .done(function (result) {
            alert(result)
            console.log(status);

        })

        .fail(function (xhr, status, error) {

            alert(`Fail!`)
            console.log("Error", xhr, status, error);

        })
});

$("body").on("click", ".userthreaddeleteButton", function () {

    let clickedId = $(this).data("id")
    console.log(clickedId)
    $.ajax({
        url: '/contents/userthreaddelete',
        method: 'DELETE',
        data: {
            clickedId
        }

    })
        .done(function (result) {
            alert(result)
            console.log(status);

        })

        .fail(function (xhr, status, error) {

            alert(`Fail!`)
            console.log("Error", xhr, status, error);

        })
});