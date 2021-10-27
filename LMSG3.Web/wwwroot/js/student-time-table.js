// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', loadPTT(0, 0, 0));


function loadPTT(year, week, step) {
    var data = { year: year, week: week, step: step };
    $.ajax(
        {
            type: 'GET',
            url: '/Student/TimeTable',
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#pttContent').html(result);
                //$('#pttModal').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}