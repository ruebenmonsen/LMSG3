// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('[id*=btnDetails]').on('click', function () {
        $.ajax({
            /* url: '@Url.Action("Details", "Course")',*/
            url:"https://localhost:44314/Courses/Details",
           /* dataType: "html",*/
            data: { "id": $(this).attr('name') },
            type: "GET",
            contentType: "application/json",
            success: function (response) {
                $('#dvPartialView').html(response);
            },
            error: function (err) {
                alert(err.responseText);
            }
        });
    });
});

$("body").on("click", "img[src*='down.jpg']", function () {
    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
    $(this).attr("src", "/img/up.jpg");
});
//Assign Click event to Minus Image.
$("body").on("click", "img[src*='up.jpg']", function () {
    $(this).attr("src", "/img/down.jpg");
    $(this).closest("tr").next().remove();
});