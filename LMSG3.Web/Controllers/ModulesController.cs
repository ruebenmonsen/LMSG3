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
using LMSG3.Data.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace LMSG3.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IRepository<Module> ModuleRepo = null;
        private readonly ILogger logger=null;
        public ModulesController(ApplicationDbContext context) 
        {
            db = context;
            this.ModuleRepo = new GenericRepository<Module>(context,   logger);
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

            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(db.Courses, "Id", "Id");
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
            if (ModelState.IsValid )
            {
                ModuleRepo.Add(module);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return db.Modules.Any(e => e.Id == id);
        }
        private void CheckDate(Module module)
        {
            var course = db.Courses.Find (module.CourseId);
            if (course != null)
            {
                if (module.StartDate < course.StartDate)
                {
                    ModelState.AddModelError("StartDate",
                                             "Module  StartDate must be less than Course StartDate");
                }
                else if (module.EndDate < course.StartDate)
                {
                    ModelState.AddModelError("EndDate",
                                             "Module  EndDate must be less than Course StartDate");
                }
            }
        }
    }
}
