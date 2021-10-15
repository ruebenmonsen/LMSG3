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
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> Index()
        {
            var model = await uow.CourseRepository.GetAllCourses();
            return View(model);

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
            if (course == null)
            {
                return NotFound();
            }

            return PartialView("Details",course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseViewModel vm)
        {

            var course = mapper.Map<Course>(vm);
            uow.CourseRepository.Add(course);
            await uow.CompleteAsync();

            return RedirectToAction(nameof(Index));


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
    }
}
