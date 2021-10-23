using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;

using LMSG3.Data;
using LMSG3.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LMSG3.Core.Models.ViewModels;
using AutoMapper;

namespace LMSG3.Web.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public CoursesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IUnitOfWork uow, IMapper mapper)
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

            ; if (course == null)
            {
                return NotFound();
            }

            return PartialView("Details", course);
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
        [HttpPost]
       // public async Task<ActionResult> CreateCoursejs(CreateCourseViewModel coursevm, List<CreateModelListViewModel> modulesetsvm)
        public async Task<IActionResult> Create(CreateCourseViewModel coursevm, List<CreateModelListViewModel> modulesetsvm)
        {
            //need to add validation 
            if (ModelState.IsValid) { 
            var courseexist = await _context.Courses.Where(c => c.Name == coursevm.Name).FirstOrDefaultAsync();
            if (courseexist != null)
            {
                //create viewbag to send the msg 
                ViewBag.Message = "Course already exists.";
                    // return Json(new { redirectToUrl = Url.Action("Index", "Courses") });
                    Response.StatusCode=500;
                    return View(coursevm);
                }
            else
            {
                var course = mapper.Map<Course>(coursevm);
                uow.CourseRepository.Add(course);
                await uow.CompleteAsync();


                foreach (var modulevm in modulesetsvm)
                {
                    var modules = new Module
                    {
                        Name = modulevm.Name,
                        Description = modulevm.Description,
                        StartDate = modulevm.StartDate,
                        EndDate = modulevm.EndDate,
                        CourseId = course.Id
                    };
                    uow.ModuleRepository.Add(modules);
                    await uow.CompleteAsync();
                }

                //return RedirectToAction(nameof(Index));
                return Json(new { redirectToUrl = Url.Action("Index", "Courses") });

            }
            }
            //return RedirectToAction("Create");
            Response.StatusCode=500;
            return View(coursevm);
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
            public async Task<IActionResult> Edit(int id, CreateCourseViewModel vm)
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
        }
    }
