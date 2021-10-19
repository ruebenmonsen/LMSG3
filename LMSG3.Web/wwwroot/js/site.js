// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function OpenAssignmentModal(id) {
    var data = { id: id };
    $.ajax(
        {
            type: 'GET',
            url: '/Student/Upload',
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#assignmentModalContent').html(result);
                $('#assignmentModal').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

document.addEventListener('DOMContentLoaded', initScrollPostion);

function initScrollPostion() {
    var scrollFirst = document.getElementById("scrollFirst");
    scrollFirst.scrollIntoView();
}