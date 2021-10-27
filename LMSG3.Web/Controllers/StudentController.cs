using LMSG3.Core.Configuration;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Models.ViewModels;
using LMSG3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LMSG3.Data;
using LMSG3.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LMSG3.Core.Models.ViewModels;
using System.Globalization;

namespace LMSG3.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly UserManager<ApplicationUser> userManager;

        public StudentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IUnitOfWork uow)
        {
            _context = context;
            this.uow = uow;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // GET: Student landing page
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);

            var currentDate = DateTime.Now;

            var assignmentTypeId = await _context.Activities.Where(a => a.ActivityType.Name == "Assignment").Select(a => a.ActivityTypeId).FirstOrDefaultAsync();

            var currentModule = await _context.Students.Where(s => s.Id == userId).Select(s => s.Course.Modules
                .Where(m => m.StartDate < currentDate && m.EndDate > currentDate).FirstOrDefault()).FirstOrDefaultAsync();
            // Include Activities for one single module.
            // TODO: fix bug with there not being a current module. Use nullable vars?
            await _context.Entry(currentModule).Collection(m => m.Activities).LoadAsync();

            var documents = await _context.Students.AsNoTracking().Where(s => s.Id == userId).Select(s => s.Documents).FirstOrDefaultAsync();
            var activities = currentModule.Activities;
            var assignmnets = activities.Where(a => a.ActivityTypeId.Equals(assignmentTypeId)).Select(a =>
            {
                var document = documents.Where(d => d?.ActivityId != null && d.ActivityId == a.Id).FirstOrDefault();
                var isSubmitted = document != default;
                return new AssignmentViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    EndDate = a.EndDate,
                    // Document.submitted > EndDate || DateNow > EndDate
                    IsOverdue = isSubmitted ? document.UploadDate > a.EndDate : currentDate > a.EndDate,
                    IsSubmitted = isSubmitted,
                    IsCurrent = currentDate < a.EndDate && !isSubmitted

                };
            }).OrderBy(a => a.EndDate).OrderByDescending(a => a.IsSubmitted).ToList();

            var moduleModel = new CurrentModuleViewModel
            {
                Assignments = assignmnets
            };

            var courseInfo = await _context.Students.AsNoTracking().Include(s => s.Course).Select(s => new CourseInfoViewModel
            {
                Name = s.Course.Name,
                Description = s.Course.Description,
                StartDate = s.Course.StartDate

            }).FirstOrDefaultAsync();

            var studentModel = await _context.Students.AsNoTracking().Where(s => s.Id == userId).Include(s => s.Documents).Include(s => s.Course)
                .ThenInclude(c => c.Modules).ThenInclude(m => m.Activities).Select(s => new StudentIndexViewModel
                {
                    Id = s.Id,
                    FName = s.FName,
                    LName = s.LName,
                    Documents = s.Documents,
                    CurrentModule = moduleModel,
                    Modules = s.Course.Modules,
                    CourseStudents = s.Course.Students,
                    CourseInfo = courseInfo

                }).AsSplitQuery().FirstOrDefaultAsync();

            return View(studentModel);
        }

        [HttpGet]
        public async Task<PartialViewResult> Upload(int? id)
        {
            var activity = await _context.Activities.FindAsync(id);

            var model = new AssignmentUploadViewModel
            {
                ActivityId = activity.Id,
                ActivityName = activity.Name,
                EndDate = activity.EndDate
            };
            return PartialView("AssignmentModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(AssignmentUploadViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var names = await _context.Activities.Where(a => a.Id == model.ActivityId).Select(a => new
            {
                Activity = a.Id,
                Module = a.Module.Id,
                Course = a.Module.Course.Id,
                Student = userManager.GetUserId(User)

            }).FirstOrDefaultAsync();

            long size = model.SubmittedFile.Length;
            string fileDirectory = $"wwwroot/Courses/{names.Course}/{names.Module}/{names.Activity}/Assignments//{names.Student}";

            if (!Directory.Exists(fileDirectory))
            {
                DirectoryInfo di = Directory.CreateDirectory(fileDirectory);
            }
            var filePath = fileDirectory + model.SubmittedFile.FileName;
            if (GetContentType(filePath) == "text/csv")
                filePath = "";

            if (filePath == "")
            {
                var document = new Document()
                {
                    UploadDate = DateTime.Now,
                    DocumentTypeId = 2,
                    ActivityId = model.ActivityId,
                    ApplicationUserId = userManager.GetUserId(User),
                    Path = filePath
                };

                if (size > 0)
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await model.SubmittedFile.CopyToAsync(stream);

                    _context.Add(document);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<PartialViewResult> TimeTable(int year, int week, int step)
        {
            // Bugs: maybe logic between weeks.

            // TODO: If we want to be able to select module, currentDate
            // need to be adjusted to the selected startdate, but if
            // the selected module is the current then the date should be Now.

            // TODO: Set limits after Course
            var currentDate = DateTime.Now;

            // If year/week is not 
            if (week < 1 || year < 1971 || year > 2100 || week > ISOWeek.GetWeeksInYear(year))
            {
                week = ISOWeek.GetWeekOfYear(currentDate);
                year = currentDate.Year;
            }

            // TODO: write tests
            // TODO: move to class or extension method

            // Basically paging: step between weeks and logic while passing years.
            if (step > 0)
            {
                week = week == ISOWeek.GetWeeksInYear(year) ? 1 : week + 1;
                year = week == 1 ? year + 1 : year;
            }
            else if (step < 0)
            {
                year = week == 1 ? year - 1 : year;
                week = week == 1 ? ISOWeek.GetWeeksInYear(year) : week - 1;
            }

            var weekNext = week == ISOWeek.GetWeeksInYear(year) ? 1 : week + 1;
            var weekPrevious = week == 1 ? ISOWeek.GetWeeksInYear(year - 1) : week - 1;

            var weekStart = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);
            var weekEnd = ISOWeek.ToDateTime(year, week, DayOfWeek.Sunday);

            var userId = userManager.GetUserId(User);

            // TODO: try for failure?
            var assignmentTypeId = await _context.ActivityTypes.AsNoTracking()
                .Where(a => a.Name == "Assignment")
                .Select(a => a.Id)
                .SingleOrDefaultAsync();

            // TODO: What happens with Max() if there are no modules?
            // Is this ternary needed?
            var courseInfo = await _context.Students.AsNoTracking()
                .Where(s => s.Id == userId)
                .Select(s => s.Course)
                .Select(c => new
                {
                    StartDate = c.StartDate,
                    EndDate = c.Modules.Any() ? c.Modules.Max(m => m.EndDate) : c.StartDate
                }).SingleOrDefaultAsync();

            // Select all activities within selected week that ain't assignments
            // Bug: Activities stretching over to next week will not be taken
            // TODO: Add module name? Add acticity name?
            // TODO: split into days? Just check if StartDate || EndDate within week.
            var studentActivities = await _context.Students.AsNoTracking()
                .Where(s => s.Id == userId)
                .SelectMany(s => s.Course.Modules)
                .SelectMany(m => m.Activities)
                .Where(a => a.StartDate >= weekStart &&
                            a.EndDate <= weekEnd &&
                            !a.ActivityTypeId.Equals(assignmentTypeId))
                .Select(a => new StudentActivityViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ActivityTypeId = a.ActivityTypeId,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    HasDocument = a.Documents.Any(),
                    IsCurrent = InDateSpan(currentDate, a.StartDate, a.EndDate),
                    InCurrentModule = InDateSpan(currentDate, a.Module.StartDate, a.Module.EndDate)
                })
                .OrderBy(a => a.StartDate)
                .ToListAsync();

            // Failed to do GroupBy on the previous query, so I had to dived it into two.
            // Dictionary of activities per day
            var studentActivitiesDictionary = studentActivities
                .GroupBy(sa => sa.StartDate.DayOfWeek)
                .ToDictionary(g => g.Key, g => g.ToList());

            var activityTypes = await _context.ActivityTypes.AsNoTracking()
                .Where(at => at.Id != assignmentTypeId)
                .ToDictionaryAsync(at => at.Id, at => at.Name);

            var currentModuleName = await _context.Students.AsNoTracking()
                .Where(s => s.Id == userId)
                .SelectMany(s => s.Course.Modules)
                .Where(m => m.StartDate <= currentDate && currentDate <= m.EndDate)
                .Select(m => m.Name).SingleOrDefaultAsync();           

            // Last hour in a day, will be set if an activity is overlapping from day to day.
            const int endHour = 24;
            var timeTable = new StudentTimeTableViewModel
            {
                Year = year,
                Week = week,
                WeekPrevious = weekPrevious,
                WeekNext = weekNext,
                HasWeekPrevious = courseInfo.StartDate < ISOWeek.ToDateTime(year, week, DayOfWeek.Monday),
                HasWeekNext = courseInfo.EndDate > ISOWeek.ToDateTime(year, week, DayOfWeek.Sunday).AddDays(1), // Add 1 a day to not mess up year
                WeekDate = ISOWeek.ToDateTime(year, week, DayOfWeek.Wednesday),
                //CurrentModuleName = currentModuleInfo?.Name ?? "", 
                CurrentModuleName = currentModuleName ?? "",
                activityStartHourMin = studentActivities.Any() ? studentActivities.Min(sa => sa.StartDate.Hour) : null,
                activityEndHourMax = studentActivities.Any() ?
                    (studentActivities.Any(sa => (sa.EndDate - sa.StartDate) > TimeSpan.FromDays(1)) ?
                    endHour : studentActivities.Max(sa => sa.EndDate.Hour))
                    : null,
                ActivityTypes = activityTypes,
                Activities = studentActivitiesDictionary
            };

            return PartialView("_TimeTablePartial", timeTable);
        }

        private static bool InDateSpan(DateTime date, DateTime start, DateTime end)
        {
            return start <= date && date <= end;
        }

        public async Task<ActionResult> ModulesList()
        {

            var userId = userManager.GetUserId(User);
            var courseId = await _context.Students.Where(s => s.Id == userId).Select(s => s.CourseId).FirstOrDefaultAsync();
            var model = await _context.Modules.Where(m => m.CourseId == courseId).ToListAsync();

            return View(model);

        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
