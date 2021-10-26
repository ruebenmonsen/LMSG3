using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;
using LMSG3.Data;
using LMSG3.Core.Repositories;
using Microsoft.Extensions.Logging;
using LMSG3.Data.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace LMSG3.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IRepository<Activity> ActivityRepo = null;
        private readonly ILogger logger = null;

        public ActivitiesController(ApplicationDbContext context)
        {
            db = context;
            this.ActivityRepo = new GenericRepository<Activity>(context, logger);
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var activities = db.Activities.Include(a => a.ActivityType).Include(a => a.Module);
            return View(await activities.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await db.Activities
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .Include(a => a.Documents).ThenInclude(a => a.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create(int? ModuleId)
        {
            ViewData["ActivityTypeId"] = new SelectList(db.Set<ActivityType>(), "Id", "Id");
            ViewData["ModuleId"] = new SelectList(db.Modules, "Id", "Id");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,ModuleId,ActivityTypeId")] Activity activity)
        {
            CheckDate(activity);
            if (ModelState.IsValid)
            {
                ActivityRepo.Add(activity);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Courses");
            }
            ViewData["ActivityTypeId"] = new SelectList(db.Set<ActivityType>(), "Id", "Id", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(db.Modules, "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await ActivityRepo.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(db.Set<ActivityType>(), "Id", "Id", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(db.Modules, "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }
            CheckDate(activity);
            if (ModelState.IsValid)
            {
                try
                {
                    ActivityRepo.Update(activity);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["ActivityTypeId"] = new SelectList(db.Set<ActivityType>(), "Id", "Id", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(db.Modules, "Id", "Id", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await db.Activities
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await ActivityRepo.FindAsync(id);
            ActivityRepo.Remove(activity);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Courses");
        }

        private bool ActivityExists(int id)
        {
            return db.Activities.Any(e => e.Id == id);
        }
        private void CheckDate(Activity activity)
        {
            var module = db.Modules.Find(activity.ModuleId);
            if (module != null)
            {
                if (module.StartDate > activity.StartDate|| module.EndDate <activity.StartDate)
                {
                    ModelState.AddModelError("StartDate",
                                             "Activity  StartDate must be within  Module Interval");
                }
                else if (module.StartDate > activity.EndDate || module.EndDate < activity.EndDate)
                {
                    ModelState.AddModelError("EndDate",
                                             "Activity  EndDate must be within  Module Interval");
                }
            }
        }
    }
}
