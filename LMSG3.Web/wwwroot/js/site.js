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

//// plus module Click event 
//$("#addModules").click(function () {
//    var html = '';
//    html += ' <div id="inputFormRow">';
//    html += ' <input type="text" name="ModuleName[]" placeholder="Name" autocomplete="off" /> &nbsp;';
//    html += '<input type="text" name="ModuleDescription[]"  placeholder="Description" autocomplete="off"> &nbsp;';
//    html += '<input type="date" name="ModuleStartDate[]"  placeholder="Start Date" autocomplete="off"> &nbsp;';
//    html += '<input type="date" name="ModuleEnddate[]"  placeholder="End Date" autocomplete="off"> &nbsp;';
   
//    html += '  <button id="removeModules" type="button" class="btn btn-danger"> - </button> <br><br>';
//    html += '</div> ';

//    $('#newRow').append(html);
//});

// remove modules
//$(document).on('click', '#removeModules', function () {
//    $(this).closest('#inputFormRow').remove();
//});

//test
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
       /* if (!ModuleStartDate[i].value >= cou.StartDate)*/
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

function checkCoursedate() {
    //const input = document.querySelector('input');
  
    var courseStartdate = $("#CourseStartDate").val();
    var moduleStartDate = this.$('#ModuleStartDate').val();
    var popup = document.getElementById("myPopup");
    
    if (moduleStartDate < courseStartdate) {
        popup.show;
       
    }
    else
     popup.classList("hide");
    
}

