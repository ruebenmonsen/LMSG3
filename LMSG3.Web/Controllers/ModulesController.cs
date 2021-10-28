using AutoMapper;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Models.ViewModels;
using LMSG3.Core.Repositories;
using LMSG3.Data;
using LMSG3.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IRepository<Module> ModuleRepo = null;
        private readonly IRepository<Activity> ActivityRepo = null;
        private readonly ILogger logger = null;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ModulesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mapper = mapper;
            ModuleRepo = new GenericRepository<Module>(context, logger);
            ActivityRepo = new GenericRepository<Activity>(context, logger);
        }

        // GET: Modules
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(await modules.ToListAsync());
        }

        // GET: Modules/Details/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await db.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }
            var documents = db.Documents.Include(d => d.DocumentType).Include(d => d.ApplicationUser).Where(d => d.ModuleId == id).OrderByDescending(d => d.UploadDate);
            List<Document> Docs = new List<Document>();
            foreach (var document in documents)
            {
                var role = await userManager.GetRolesAsync(document.ApplicationUser);
                if (role[0].ToString() == "Teacher")
                    Docs.Add(document);
            }
            module.Documents = Docs;

            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create(int? CourseId)
        {
            //ViewData["CourseId"] = new SelectList(db.Courses, "Id", "Id");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,CourseId,Activities")] Module module)
        {
            CheckDate(module);
            if (ModelState.IsValid)
            {
                ModuleRepo.Add(module);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Courses");
            }
            ViewData["CourseId"] = new SelectList(db.Courses, "Id", "Id", module.CourseId);
            return View(module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await ModuleRepo.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(db.Courses, "Id", "Id", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (id != module.Id)
            {
                return NotFound();
            }
            CheckDate(module);
            if (ModelState.IsValid)
            {
                try
                {
                    ModuleRepo.Update(module);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Courses");
            }
            ViewData["CourseId"] = new SelectList(db.Courses, "Id", "Id", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await db.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await ModuleRepo.FindAsync(id);
            ModuleRepo.Remove(module);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Courses");
        }

        private bool ModuleExists(int id)
        {
            return db.Modules.Any(e => e.Id == id);
        }
        private bool CheckDate(Module module)
        {
            var course = db.Courses.Find(module.CourseId);
            var LastModuleEndDate = db.Modules
                .Where(m => m.CourseId == course.Id)
                .Select(m => m.EndDate).Max();
            if (course != null)
            {
                if (module.StartDate < course.StartDate)
                {
                    //ModelState.AddModelError("StartDate",
                    //                         "Module  StartDate must be less than Course StartDate");
                    return false;
                }
                if (module.EndDate < course.StartDate)
                {
                    // ModelState.AddModelError("EndDate",
                    //                          "Module  EndDate must be less than Course StartDate");
                    return false;
                }
                if (module.StartDate < LastModuleEndDate)
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }
        private bool CheckDate(Activity activity, DateTime Module_StartDate, DateTime Module_EndDate)
        {
            if (Module_StartDate > activity.StartDate || Module_EndDate < activity.StartDate)
            {
                //ModelState.AddModelError("StartDate",
                //                         "Activity  StartDate must be within  Module Interval");
                return false;
            }
            if (Module_StartDate > activity.EndDate || Module_EndDate < activity.EndDate)
            {
                //ModelState.AddModelError("EndDate",
                //                         "Activity  EndDate must be within  Module Interval");
                return false;
            }
            return true;
        }

        public ActionResult DisplayNewActivitySet()
        {
            return PartialView("CreateActivityPartial");
        }
        //[HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> CreateModule(CreateModuleViewModel Modulevm, List<CreateActivityListViewModel> activitysetsvm)
        {

            var Module = mapper.Map<Module>(Modulevm);
            if (CheckDate(Module))
            {
                ModuleRepo.Add(Module);
                await db.SaveChangesAsync();

                foreach (var activityvm in activitysetsvm)
                {
                    var activity = new Activity
                    {
                        Name = activityvm.Name,
                        Description = activityvm.Description,
                        StartDate = activityvm.StartDate,
                        EndDate = activityvm.EndDate,
                        ActivityTypeId = activityvm.ActivityTypeId,
                        ModuleId = Module.Id
                    };
                    if (CheckDate(activity, Module.StartDate, Module.EndDate))
                    {
                        ActivityRepo.Add(activity);
                        await db.SaveChangesAsync();
                    }
                }
            }
          //  return RedirectToAction("Index", "Courses");
            return Json(new { redirectToUrl = Url.Action("Index", "Courses") });

        }
      
    }
}
