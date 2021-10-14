using LMSG3.Core.Configuration;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using LMSG3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            db = context;
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        [Authorize(Roles = "Teacher")]
        // GET: UsersController
        public async Task<IActionResult> Index()
        {
            var usersDto = new List<UserDto>();
            var users = userManager.Users.OrderBy(u => u.FName);

            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);

                var userDto = new UserDto()
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                    Role = role[0].ToString()
                };
                usersDto.Add(userDto);
            }
            
            if (usersDto == null)
            {
                return NotFound();
            }
            return View(usersDto);
        }

        // GET: UsersController/Details/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // GET: UsersController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FName,LName,Email,CourseId,Role")]  UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (userDto.Role == "Student")
                    {
                        var student =await userManager.FindByIdAsync(id) as Student;
                        student.Id = userDto.Id;
                        student.FName = userDto.FName;
                        student.LName = userDto.LName;
                        student.Email = userDto.Email;
                        student.CourseId = userDto.CourseId;

                        await userManager.UpdateAsync(student);
                    }
                    else
                    {
                        var user = await userManager.FindByIdAsync(id);
                        user.Id = userDto.Id;
                        user.FName = userDto.FName;
                        user.LName = userDto.LName;
                        user.Email = userDto.Email;

                        await userManager.UpdateAsync(user);
                    }
                      
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userDto.Id))
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
            return View(userDto);
        }
        private bool UserExists(string id)
        {
            return db.ApplicationUser.Any(e => e.Id == id);
        }
        // GET: UsersController/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user =await GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var user = await db.ApplicationUser.FindAsync(id);
                db.ApplicationUser.Remove(user);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        async Task<UserDto> GetUser(string id)
        {
            var user = db.ApplicationUser.Where(u => u.Id.Equals(id)).FirstOrDefault();

            var role = await userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                FName = user.FName,
                LName = user.LName,
                Email = user.Email,
                Role = role[0].ToString()

            };
            if (role[0].ToString() == "Student")
            {
                var course = db.Courses.Where(m =>
                    db.Students.Where(u => u.Id == id).Any(u => u.CourseId == m.Id)).FirstOrDefault();
                userDto.CourseId = course.Id;
                userDto.Course = course;
            }
                return userDto;
        }
    }
}
