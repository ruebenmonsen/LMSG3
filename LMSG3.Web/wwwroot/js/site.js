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

function OpenStudentsAssignmentsModal(id) {
    var data = { id: id };
    $.ajax(
        {
            type: 'GET',
            url: '/Documents/GetAssignments',
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#StudentsAssignmentsModalContent').html(result);
                $('#StudentsAssignmentsModal').modal('show');
            },
            error: function (er) {
                alert(er);
            }
        });
}

function OpenDocumentModal(id, entityName) {
    var data = { id: id, entityName: entityName };
    $.ajax(
        {
            type: 'GET',
            url: '/Documents/Upload',
            contentType: 'application/json; charset=utf=8',
            data: data,
            success: function (result) {
                $('#DocumentModalContent').html(result);
                $('#DocumentModal').modal('show');
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

        data: { "coursevm": course, "modulesetsvm": moduleSets },
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
///////////////////////////Create Module///////////////////////////////////
$(document).on('click', '#Btn_AddActivityset', function (e) {
    $.ajax({
        url: '/Modules/DisplayNewActivitySet',
        success: function (partialView) {
            $('.ActivitiesListSet').append(partialView);
        }
    });
});


$(document).on('click', '#Btn_DeleteActivityset', function () {
    $(this).parent().parent().remove();
});

$(document).on('click', '#BtnCreateModule', function (e) {
    var module = getModule();
    var activitySets = getActivitysets();

    $.ajax({
        type: 'POST',
        // url: '@Url.Action("CreateCourse", "Courses")',
        url: "https://localhost:44314/Modules/CreateModule",

        data: { "Modulevm": module, "activitysetsvm": activitySets },
        success: function (response) {
            window.location.href = response.redirectToUrl;
            alert('successfully module created');
        }
        ,
        error: function (err) {
            alert('error');
        }
    });
});

function getModule() {
    var module = {
        Name: $("#ModuleName").val(),
        Description: $("#ModuleDescription").val(),
        StartDate: $("#ModuleStartDate").val(),
        EndDate: $("#ModuleEndDate").val(),
        CourseId: $("#ModuleCourseId").val()
    };
    return module;
}


function getActivitysets() {
    activitySets = [];
    var ActivityName = document.querySelectorAll('#ActivityName');
    var ActivityDescription = document.querySelectorAll('#ActivityDescription');
    var ActivityStartDate = document.querySelectorAll('#ActivityStartDate');
    var ActivityEndDate = document.querySelectorAll('#ActivityEndDate');
    var ActivityTypeId = document.querySelectorAll('#Activity_TypeId');
    for (var i = 0; i < ActivityName.length; i++) {
        /* if (!ModuleStartDate[i].value >= cou.StartDate)*/
        if (ActivityName[i].value != '') {
            activitySets.push({
                Name: ActivityName[i].value,
                Description: ActivityDescription[i].value,
                StartDate: ActivityStartDate[i].value,
                EndDate: ActivityEndDate[i].value,
                ActivityTypeId: ActivityTypeId[i].value
            });
        }
    }
    return activitySets;
}

/*Filtering student assignments*/
const allTab = document.querySelector('#assignments-all')
const overdueTab = document.querySelector('#assignments-overdue')
const submittedTab = document.querySelector('#assignments-submitted')
const upcomingTab = document.querySelector('#assignments-upcoming')

const allTabs = document.querySelectorAll('.assignment-tab')
const tabsArray = Array.from(allTabs)

const listItems = document.querySelectorAll('.assignment-list-item')
const itemsArray = Array.from(listItems)

function switchActiveTab() {
    tabsArray.forEach(a => {
        if (a.classList.contains('active')) {
            a.classList.remove('active')
        }
    });
}

allTab.addEventListener('click', allAssignemnts)
function allAssignemnts() {
    switchActiveTab();
    this.classList.add('active');
    listItems.forEach(li => {
        li.classList.remove('not-shown')
    })
}

overdueTab.addEventListener('click', overdue)
function overdue() {
    switchActiveTab();
    this.classList.add('active');
    listItems.forEach(li => {
        if (!li.classList.contains('overdue-assignment')) {
            li.classList.add('not-shown')
        }
        else
            li.classList.remove('not-shown')
    })
}

submittedTab.addEventListener('click', submitted)
function submitted() {
    switchActiveTab();
    this.classList.add('active');
    listItems.forEach(li => {
        if (!li.classList.contains('submitted-assignment')) {
            li.classList.add('not-shown')
        }
        else
            li.classList.remove('not-shown')
    })
}

upcomingTab.addEventListener('click', upcoming)
function upcoming() {
    switchActiveTab();
    this.classList.add('active');
    listItems.forEach(li => {
        if (!li.classList.contains('upcoming-assignment')) {
            li.classList.add('not-shown')
        }
        else
            li.classList.remove('not-shown')
    })
}