using AutoMapper;
using LMSG3.Core.Configuration;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Models.ViewModels;
using LMSG3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public CoursesController(UserManager<ApplicationUser> userManager,ApplicationDbContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            this.uow = uow;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mapper = mapper;
        }

        // GET: Courses
       
        public async Task<IActionResult> Index()
        {
            var model = await uow.CourseRepository.GetAllCourses(true);
            

            return View(mapper.Map<IndexCourseViewModel>(model));

        }

        // GET: Courses/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await uow.CourseRepository.GetCourse(id, true);
           
;            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(CreateCourseViewModel vm)
        //{

        //    var course = mapper.Map<Course>(vm);
        //    var modules = mapper.Map<List<Module>>(vm.Modelslist);
        //    uow.CourseRepository.Add(course);
        //    await uow.CompleteAsync();
        //    foreach (var item in modules)
        //    {
        //        uow.ModuleRepository.Add(item);
        //        await uow.CompleteAsync();
        //    }
           

        //    return RedirectToAction(nameof(Index));


        //}
        //[HttpPost]
        public async Task<ActionResult> CreateCourse(CreateCourseViewModel coursevm, List<CreateModelListViewModel> modulesetsvm)
        {
            var course = mapper.Map<Course>(coursevm);
            uow.CourseRepository.Add(course);
            await uow.CompleteAsync();

            foreach (var modulevm in modulesetsvm)
            {
                var modules = new Module 
                {
                Name=modulevm.Name,
                Description=modulevm.Description,
                StartDate=modulevm.StartDate,
                EndDate=modulevm.EndDate,
                CourseId=course.Id
                };
                if(CheckDate(modules,course.StartDate))
                { 
                uow.ModuleRepository.Add(modules);
                await uow.CompleteAsync();
                }
            }
            
            //return RedirectToAction(nameof(Index));
            return Json(new { redirectToUrl = Url.Action("Index", "Courses") });

        }

        public ActionResult DisplayNewModuleSet()
        {
            return PartialView("CreateModulePartial");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await uow.CourseRepository.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }   
        
     

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CreateCourseViewModel vm)
        {
            
            if (id != vm.Id)
            {
                return BadRequest();
            }
            var course = await uow.CourseRepository.FindAsync(vm.Id);
            if (course == null)
                return NotFound();
            mapper.Map(vm, course);
           // uow.CourseRepository.Update(course);
            await uow.CompleteAsync();
            

            return RedirectToAction("Index");
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await uow.CourseRepository.FindAsync(id);
               
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await uow.CourseRepository.FindAsync(id);
            if (course == null)
                return NotFound();
            uow.CourseRepository.Remove(course);
            await uow.CompleteAsync();

            return RedirectToAction(nameof(Index));

        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        private bool CheckDate(Module module ,DateTime Course_StartDate)
        {
            var LastModuleEndDate = _context.Modules.Select(m => m.EndDate).Max();

                if (module.StartDate < Course_StartDate)
                {
                    //ModelState.AddModelError("StartDate",
                    //                         "Module  StartDate must be less than Course StartDate");
                    return false;
                }
                if (module.EndDate < Course_StartDate)
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

        [HttpGet]
        public async Task<PartialViewResult> Upload(int? id)
        {
            var course = await _context.Courses.FindAsync(id);

            var model = new DocumentUploadViewModel
            {
                Id = course.Id,
                Name = course.Name
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
                Activity = a.Name,
                Module = a.Module.Name,
                Course = a.Module.Course.Name

            }).FirstOrDefaultAsync();

            long size = model.SubmittedFile.Length;
            string fileDirectory = $"wwwroot/Courses/{names.Course}/{names.Module}/{names.Activity}/Assignments/";

            if (!Directory.Exists(fileDirectory))
            {
                DirectoryInfo di = Directory.CreateDirectory(fileDirectory);
            }
            var filePath = fileDirectory + model.SubmittedFile.FileName;

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
            return RedirectToAction(nameof(Index));
        }
    }
}
