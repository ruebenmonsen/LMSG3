using LMSG3.Core.Models.Entities;
using LMSG3.Core.Models.ViewModels;
using LMSG3.Core.Repositories;
using LMSG3.Data;
using LMSG3.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private object model;
   
        private readonly IRepository<Document> DocumentRepo = null;
        private readonly ILogger logger = null;

        public DocumentsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment _environment)
        {
            db = context;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.DocumentRepo = new GenericRepository<Document>(context, logger);

        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = db.Documents.Include(d => d.Activity).Include(d => d.ApplicationUser).Include(d => d.Course).Include(d => d.DocumentType).Include(d => d.Module);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Documents
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<PartialViewResult> GetAssignments(int? id)
        {
            var documents = db.Documents.Include(d => d.Activity).Include(d => d.DocumentType).Include(d => d.ApplicationUser)
                                        .Where(d => d.ActivityId == id && d.DocumentTypeId==2).OrderByDescending(d=>d.UploadDate);
            List<StudentAssignmentViewModel> model = new List<StudentAssignmentViewModel>();
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    var role = await userManager.GetRolesAsync(document.ApplicationUser);
                    if (role[0].ToString() == "Student")
                    {
                        var doc = new StudentAssignmentViewModel()
                        {
                            Id = document.Id,
                            AssignmentName = document.Name,
                            StudentName = document.ApplicationUser.FName + " " + document.ApplicationUser.LName,
                            UploadDate = document.UploadDate,
                            Path = document.Path
                        };
                        model.Add(doc);
                    }
                }
            }

            return PartialView("StudentsAssignmentsModal", model);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await db.Documents
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



        // GET: Documents/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await db.Documents
                                            .Include(d => d.Activity)
                                            .Include(d => d.ApplicationUser)
                                            .Include(d => d.Course)
                                            .Include(d => d.DocumentType)
                                            .Include(d => d.Module).FirstOrDefaultAsync(d => d.Id == id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DocumentTypeId")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var doc = await DocumentRepo.FindAsync(id);
                    doc.Id = document.Id;
                    doc.Name = document.Name;
                    doc.Description = document.Description;
                    doc.DocumentTypeId = document.DocumentTypeId;


                    DocumentRepo.Update(doc);
                    await db.SaveChangesAsync();
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

            return View(document);
        }

        // GET: Documents/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await db.Documents
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await DocumentRepo.FindAsync(id);
            DocumentRepo.Remove(document);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Courses");
        }

        private bool DocumentExists(int id)
        {
            return db.Documents.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<PartialViewResult> Upload(int? id, string entityName)
        {

            if (entityName == "Courses")
            {
                var course = await db.Courses.FindAsync(id);

                model = new DocumentUploadViewModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    EntityName = "Courses"
                };
            }
            else if (entityName == "Modules")
            {
                var module = await db.Modules.FindAsync(id);

                model = new DocumentUploadViewModel
                {
                    Id = module.Id,
                    Name = module.Name,
                    EntityName = "Modules"
                };
            }
            else if (entityName == "Activities")
            {
                var activity = await db.Activities.FindAsync(id);

                model = new DocumentUploadViewModel
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    EntityName = "Activities"
                };
            }
            return PartialView("DocumentModal", model);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Upload(DocumentUploadViewModel model)
        {
            long size = 0;
            var filePath = "";
            Document document = new();


            if (ModelState.IsValid)
            {
                if (model.EntityName == "Courses")
                {
                    var names = await db.Courses.Where(a => a.Id == model.Id).Select(a => new
                    {
                        Course = a.Id

                    }).FirstOrDefaultAsync();

                    size = model.SubmittedFile.Length;
                    string fileDirectory = $"wwwroot/Courses/{names.Course}/";

                    if (!Directory.Exists(fileDirectory))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(fileDirectory);
                    }
                    filePath = fileDirectory + model.SubmittedFile.FileName;

                    if (GetContentType(filePath) == "text/csv")
                        filePath = "";

                    document = new Document()
                    {
                        UploadDate = DateTime.Now,
                        DocumentTypeId = model.DocumentTypeId,
                        CourseId = model.Id,
                        Name = model.DocumentName,
                        Description = model.Description,
                        ApplicationUserId = userManager.GetUserId(User),
                        Path = filePath
                    };
                }

                else if (model.EntityName == "Modules")
                {
                    var names = await db.Modules.Where(a => a.Id == model.Id).Select(a => new
                    {
                        Module = a.Id,
                        Course = a.Course.Id

                    }).FirstOrDefaultAsync();

                    size = model.SubmittedFile.Length;
                    string fileDirectory = $"wwwroot/Courses/{names.Course}/{names.Module}/";

                    if (!Directory.Exists(fileDirectory))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(fileDirectory);
                    }
                    filePath = fileDirectory + model.SubmittedFile.FileName;
                    if (GetContentType(filePath) == "text/csv")
                        filePath = "";

                    document = new Document()
                    {
                        UploadDate = DateTime.Now,
                        DocumentTypeId = model.DocumentTypeId,
                        ModuleId = model.Id,
                        Name = model.DocumentName,
                        Description = model.Description,
                        ApplicationUserId = userManager.GetUserId(User),
                        Path = filePath
                    };
                }

                else if (model.EntityName == "Activities")
                {
                    var names = await db.Activities.Where(a => a.Id == model.Id).Select(a => new
                    {
                        Activity = a.Id,
                        Module = a.Module.Id,
                        Course = a.Module.Course.Id

                    }).FirstOrDefaultAsync();

                    size = model.SubmittedFile.Length;
                    string fileDirectory = $"wwwroot/Courses/{names.Course}/{names.Module}/{names.Activity}/";

                    if (!Directory.Exists(fileDirectory))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(fileDirectory);
                    }
                    filePath = fileDirectory + model.SubmittedFile.FileName;

                    if (GetContentType(filePath) == "text/csv")
                        filePath = "";

                    document = new Document()
                    {
                        UploadDate = DateTime.Now,
                        DocumentTypeId = model.DocumentTypeId,
                        ActivityId = model.Id,
                        Name = model.DocumentName,
                        Description = model.Description,
                        ApplicationUserId = userManager.GetUserId(User),
                        Path = filePath
                    };
                }
                if (filePath != "")
                {
                    if (size > 0)
                    {
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await model.SubmittedFile.CopyToAsync(stream);

                        DocumentRepo.Add(document);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index", "Courses");
        }


        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DownloadAsync(string path)
        {

            if (path == null)
            {
                return RedirectToAction("Index", "Courses");
            }

            var newpath = Path.Combine(
                           Directory.GetCurrentDirectory(), path);

            var memory = new MemoryStream();
            using (var stream = new FileStream(newpath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(newpath), Path.GetFileName(newpath));

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
