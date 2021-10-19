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

//Assign Click event to up Down image.
$("body").on("click", "img[src*='down.jpg']", function () {
    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
    $(this).attr("src", "/img/up.jpg");
});
//Assign Click event to up Image.
$("body").on("click", "img[src*='up.jpg']", function () {
    $(this).attr("src", "/img/down.jpg");
    $(this).closest("tr").next().remove();
});
var i = 0;
var j = 0;
// plus module Click event 
$("#addModules").click(function () {
    var html = '';
    html += ' <div>';
    html += ' <div id="inputFormRow">';
    html += ' <input type="text" name="ModuleName[i]" placeholder="Name" autocomplete="off" /> &nbsp;';
    html += '<input type="text" name="ModuleDescription[i]"  placeholder="Description" autocomplete="off"> &nbsp;';
    html += '<input type="date" name="ModuleStartDate[i]"  placeholder="Start Date" autocomplete="off"> &nbsp;';
    html += '<input type="date" name="ModuleEnddate[i]"  placeholder="End Date" autocomplete="off"> &nbsp;';
   
    html += '  <button id="removeModules" type="button" class="btn btn-danger"> - </button> <br><br>';
    html += '</div> ';
    html += '  <button id="addActivity" type="button" class="btn btn-info"> Add Activity </button> <br><br>';
    html += '</div> ';

    $('#newRow').append(html);
});
$("#addActivity").click(function () {
    var html = '';
    html += ' <div>';
    html += ' <div id="inputFormRow">';
    html += ' <div class="col-sm-2"></div>';

    html += ' <input type="text" name="ActivityName[i][j]" placeholder="Name" autocomplete="off" /> &nbsp;';
    html += '<input type="text" name="ActivityDescription[i][j]"  placeholder="Description" autocomplete="off"> &nbsp;';
    html += '<input type="date" name="ActivityStartDate[i][j]"  placeholder="Start Date" autocomplete="off"> &nbsp;';
    html += '<input type="date" name="ActivityEnddate[i][j]"  placeholder="End Date" autocomplete="off"> &nbsp;';
   
    html += '  <button id="removeModules" type="button" class="btn btn-danger"> - </button> <br><br>';
    html += '</div> ';
    html += '  <button id="addActivity" type="button" class="btn btn-info"> + </button> <br><br>';
    html += '</div> ';

    $('#newRow').append(html);
});

// remove modules
$(document).on('click', '#removeModules', function () {
    $(this).closest('#inputFormRow').remove();
});

//for adding module-----
$(document).on('click', '#Btn_AddModuleset', function (e) {
    $.ajax({
        url: '/Courses/DisplayNewModuleSet',
        success: function (partialView) {
            $('.ModulesListSet').append(partialView);
        }
    });
});

$(document).on('click', '#Btn_DeleteModuleset', function () {
    $(this).parent().parent().remove();
});

//for adding and deleting activity
$(document).on('click', '#Btn_AddActivityset', function (e) {
    $.ajax({
        url: '/Courses/DisplayNewActivitySet',
        success: function (partialView) {
            $('.ActivityListSet').append(partialView);
        }
    });
});

$(document).on('click', '#Btn_DeleteActivityset', function () {
    $(this).parent().parent().remove();
});

$(document).on('click', '#BtnCreate', function (e) {
    var course = getCourse();
    var moduleSets = getModulesets();

    $.ajax({
        type: 'POST',
       // url: '@Url.Action("CreateCourse", "Courses")',
        url: "https://localhost:44314/Courses/CreateCourse",
       
        data: { "coursevm": course, "modulesetsvm":moduleSets },
        success: function (response) {
            window.location.href = response.redirectToUrl;
            alert('successfully course created');
        }
        ,
        error: function (err) {
           alert('error');
        }
    });
});

function getCourse() {
    var course = {
        Name: $("#CourseName").val(),
        StartDate: $("#CourseStartDate").val(),
        Description: $("#CourseDescription").val()
    };
    return course;
}

function getModulesets() {
    moduleSets = [];
    var ModuleName = document.querySelectorAll('#ModuleName');
    var ModuleDescription = document.querySelectorAll('#ModuleDescription');
    var ModuleStartDate = document.querySelectorAll('#ModuleStartDate');
    var ModuleEndDate = document.querySelectorAll('#ModuleEndDate');
    for (var i = 0; i < ModuleName.length; i++) {
        if (ModuleName[i].value != '') {
            moduleSets.push({
                Name: ModuleName[i].value,
                Description: ModuleDescription[i].value,
                StartDate: ModuleStartDate[i].value,
                EndDate: ModuleEndDate[i].value
                
            });
        }
    }
    return moduleSets;
}

