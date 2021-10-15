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