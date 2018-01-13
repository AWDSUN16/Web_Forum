$(document).ready(function () {
    updateNavBar();
    updateThreadDiv();
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
            { $("#adminButton").show();}
           
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
            "email": $("#userLoginForm [name=UserLoginEmail]").val()
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
            updateThreadDiv();
            console.log(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");
            console.log("Error", xhr, status, error)
        });
});

function updateThreadDiv() {

    $.ajax({
        url: '/contents/getAllThreads',
        method: 'GET'
    })
        .done(function (result) {

            var threads = fillTableWithThreads(result);
            $("#threadDiv tbody").empty();
            $("#threadDiv tbody").append(threads);

            console.log(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");
            console.log("Error", xhr, status, error)
        });

}

function fillTableWithThreads(result) {

    var html = "";

    $.each(result, function (key, thread) {
        html += '<tr>';
        html += '<td style="border: 1px solid black;">' + '<a href="#threadDataDiv" class="threadLink" thread-id="' + thread.id + '">' + thread.title + '</a>' + '</td>';
        html += '<td style="border: 1px solid black;">' + thread.dateOfCreation + '</td>';
        html += '<td style="border: 1px solid black;">' + thread.amountOfReplies + '</td>';
        html += '<td style="border: 1px solid black;">' + thread.amountOfViews + '</td>';
        html += '</tr>';
    });

    return html;
}

$(document).on("click", "a.threadLink", function () {

    var id = $(this).attr("thread-id");

    $.ajax({
        url: '/contents/threads/',
        method: 'GET',
        data: {
            "id": id
        }
    })
        .done(function (result) {
            buildThreadDataTable(result);
            fillTableWithThreadPosts(result);
            buildThreadPostForm(result);

            console.log(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");
            console.log("Error", xhr, status, error)
        });

});

function buildThreadDataTable(result) {
    emptythreadDataDiv();

    var html = '<a href="/index.html">' + 'Tillbaka' + '</a>' + ' > ' + result.title;
    html += '<h1>' + result.title + '</h1>';

    $("#threadDataDiv").append(html);
}

function fillTableWithThreadPosts(result) {

    var id = result.id;
    console.log(id);

    $.ajax({
        url: '/contents/threads/' + id + '/posts',
        method: 'GET',
        data: {
            "id": id
        }
    })
        .done(function (result) {

            var html = "";

            $.each(result, function (key, post) {
                html += '<tr>';
                html += '<td style="border: 1px solid black;">' + post.createdBy + '</td>';
                html += '<td style="border: 1px solid black;">' + post.dateOfCreation + '</td>';
                html += '<td style="border: 1px solid black;">' + post.content + '</td>';
                html += '<td><button id="id_delete_for_'  + post.id +'"class="deletePostButton_">Delete</button></td>';
                html += '<td><button id="id_edit_for_' + post.id + '" class="editPostButton">Edit</button></td>';
                html += '</tr>';
            });

            $("#threadDataDiv").append(html);

            console.log(result);
        })

        .fail(function (xhr, status, error) {
            alert("fail!");

            console.log("Error", xhr, status, error)
        });

}
function buildThreadPostForm(result) {
    //IMPORTANT: put the thread Id in the post-form for the Posts in a Thread
    var html = '<div id="threadPostForm" thread-id="' + result.id + '">'
    html += '<textarea name="CreatePostContent" placeholder="Skriv ett inlägg..." ></textarea>';
    html += '<button class="sendThreadForm">Svara</button>';
    html += '</div >';

    $("#threadDataDiv").append(html);
}

function emptythreadDataDiv() {
    $("#threadDataDiv").empty();
}

$(document).on("click", "button.sendThreadForm", function () {
    //Get the thread.Id from the thread-id attribute inside the post-form
    var id = $("#threadPostForm").attr('thread-id');

    $.ajax({
        url: '/contents/threads/' + id + '/posts',
        method: 'POST',
        dataType: {
            "id": id
        }
    })
        .done(function (result) {
            alert(result);

        })

        .fail(function (xhr, status, error) {
            alert("fail!");
        });

});
