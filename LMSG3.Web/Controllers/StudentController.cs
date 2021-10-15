﻿using System;
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

            var userId = userManager.GetUserId(User);
            var courseInfo = await _context.Students.Include(s => s.Course).Select(s => new CourseInfoViewModel
            {
                Name = s.Course.Name,
                Description = s.Course.Description,
                StartDate = s.Course.StartDate
            }).FirstOrDefaultAsync();

            //Id = "8e2d4df3-6adf-4517-bd6f-d52d8ea05a73"
            //userId = "712a4198-1a73-47f8-8046-bc5656aad62f"

            var student2 = await _context.Students.Where(s => s.Id == userId).FirstOrDefaultAsync();

            var student = await _context.Students.Where(s => s.Id == userId).Include(s => s.Documents).Include(s => s.Course)
                .ThenInclude(c => c.Modules).ThenInclude(m => m.Activities).Select(s => new StudentIndexViewModel 
                { 
                    FName = s.FName,
                    LName = s.LName,
                    Documents = s.Documents,
                    Assignments = s.Course.Modules.SelectMany(m => m.Activities).Where(a => a.ActivityType.Name.Equals("Assignment")).ToList(),
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
