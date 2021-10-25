using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;
using LMSG3.Data;

namespace LMSG3.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Documents.Include(d => d.Activity).Include(d => d.ApplicationUser).Include(d => d.Course).Include(d => d.DocumentType).Include(d => d.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Activity)
                .Include(d => d.ApplicationUser)
                .Include(d => d.Course)
                .Include(d => d.DocumentType)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Description");
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Id");
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,UploadDate,DocumentTypeId,ApplicationUserId,CourseId,ModuleId,ActivityId")] Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Courses");
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Description", document.ActivityId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", document.ApplicationUserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", document.CourseId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Id", document.DocumentTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description", document.ModuleId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                                            .Include(d => d.Activity)
                                            .Include(d => d.ApplicationUser)
                                            .Include(d => d.Course)
                                            .Include(d => d.DocumentType)
                                            .Include(d => d.Module).FirstOrDefaultAsync(d => d.Id==id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Description", document.ActivityId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", document.ApplicationUserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", document.CourseId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Id", document.DocumentTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description", document.ModuleId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,UploadDate,DocumentTypeId,ApplicationUserId,CourseId,ModuleId,ActivityId")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Description", document.ActivityId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", document.ApplicationUserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", document.CourseId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Id", document.DocumentTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description", document.ModuleId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Activity)
                .Include(d => d.ApplicationUser)
                .Include(d => d.Course)
                .Include(d => d.DocumentType)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Courses");
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
