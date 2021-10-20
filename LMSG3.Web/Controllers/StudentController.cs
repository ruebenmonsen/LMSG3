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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            await _context.Entry(currentModule).Collection(m => m.Activities).LoadAsync();
            var documents = await _context.Students.Where(s => s.Id == userId).Select(s => s.Documents).FirstOrDefaultAsync();
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

            var courseInfo = await _context.Students.Include(s => s.Course).Select(s => new CourseInfoViewModel
            {
                Name = s.Course.Name,
                Description = s.Course.Description,
                StartDate = s.Course.StartDate

            }).FirstOrDefaultAsync();

            var studentModel = await _context.Students.Where(s => s.Id == userId).Include(s => s.Documents).Include(s => s.Course)
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

                }).FirstOrDefaultAsync();

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
        public async Task<IActionResult> Upload(IFormFile incomingFile, AssignmentUploadViewModel model)
        {
            var names = await _context.Activities.Where(a => a.Id == model.ActivityId).Select(a => new
            {
                Activity = a.Name,
                Module = a.Module.Name,
                Course = a.Module.Course.Name

            }).FirstOrDefaultAsync();

            long size = incomingFile.Length;
            string fileDirectory = $"wwwroot/Courses/{names.Course}/{names.Module}/{names.Activity}/";

            if (!Directory.Exists(fileDirectory))
            {
                DirectoryInfo di = Directory.CreateDirectory(fileDirectory);
            }
            var filePath = fileDirectory + incomingFile.FileName;

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
                await incomingFile.CopyToAsync(stream);

                _context.Add(document);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> ModulesList()
        {

            var userId = userManager.GetUserId(User);
            var courseId = await _context.Students.Where(s => s.Id == userId).Select(s => s.CourseId).FirstOrDefaultAsync();
            var model = await _context.Modules.Where(m => m.CourseId == courseId).ToListAsync();

            return View(model);

        }
    }
}
