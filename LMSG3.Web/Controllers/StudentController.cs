using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;

using LMSG3.Data;
using LMSG3.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LMSG3.Core.Models.ViewModels;

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

        // GET: Courses
       
        public async Task<IActionResult> Index()
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var userId = userManager.GetUserId(User);
            var courseInfo = await _context.Students.Include(s => s.Course).Select(s => new CourseInfoViewModel
            {
                Name = s.Course.Name,
                Description = s.Course.Description,
                StartDate = s.Course.StartDate
            }).FirstOrDefaultAsync();

            //var student3 = await _context.Students.Where(s => s.Id == userId).Include(s => s.Documents).Include(s => s.Course)
            //    .ThenInclude(c => c.Modules).ThenInclude(m => m.Activities).FirstOrDefaultAsync();

            //var assignments = student3.Course.Modules.SelectMany(m => m.Activities).Where(a => a.ActivityType.Name.StartsWith("A")).ToList();

            //var module = student3.Course.Modules.Where(m => m.StartDate < DateTime.Now && m.EndDate > DateTime.Now).FirstOrDefault();

            //var module = await _context.Students.Where(s => s.Id == userId).Include(s => s.Course).Select(s => s.Course)
            //                                    .Include(c => c.Modules.Where(m => m.StartDate < DateTime.Now && m.EndDate > DateTime.Now))
            //                                    .FirstOrDefaultAsync();

            //Id = "8e2d4df3-6adf-4517-bd6f-d52d8ea05a73"
            //userId = "712a4198-1a73-47f8-8046-bc5656aad62f"

            var currentDate = DateTime.Now;

            //var student = await _context.Students.FindAsync(userId);
            var assignmentTypeId = _context.Activities.Where(a => a.ActivityType.Name == "Assignment").Select(a => a.ActivityTypeId);
            var modules = await _context.Students.Where(s => s.Id == userId).Select(s => s.Course.Modules).FirstOrDefaultAsync();
            var currentModule = modules.Where(m => m.StartDate < currentDate && m.EndDate > currentDate).FirstOrDefault();
            var documents = await _context.Students.Where(s => s.Id == userId).Select(s => s.Documents).FirstOrDefaultAsync();
            var activities = currentModule.Activities;
            var assignmnets = activities.Where(a => a.ActivityTypeId.Equals(assignmentTypeId)).Select(a => new AssignmentViewModel { 
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                EndDate = a.EndDate,
                IsOverdue = documents.Where(d => d.ActivityId == a.Id).FirstOrDefault().UploadDate < a.EndDate,
                IsSubmitted = documents.Where(d => d.ActivityId == a.Id).Any()

            });

            var student2 = await _context.Students.Where(s => s.Id == userId).FirstOrDefaultAsync();

            var student = await _context.Students.Where(s => s.Id == userId).Include(s => s.Documents).Include(s => s.Course)
                .ThenInclude(c => c.Modules).ThenInclude(m => m.Activities).Select(s => new StudentIndexViewModel 
                { 
                    Id = s.Id,
                    FName = s.FName,
                    LName = s.LName,
                    Documents = s.Documents,
                    //Assignments = s.Course.Modules.SelectMany(m => m.Activities).Where(a => a.ActivityType.Name.StartsWith("A")).ToList(),
                    //Activities = s.Course.Modules.SelectMany(m => m.Activities).Where(a => !a.ActivityType.Name.Equals("Assignment")).ToList(),
                    Modules = s.Course.Modules,
                    CourseStudents = s.Course.Students,
                    CourseInfo = courseInfo

                }).FirstOrDefaultAsync();

            Console.WriteLine("tst");
            return View(student);
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
