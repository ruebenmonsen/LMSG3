using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {

            if(User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }
            else 
            {
                return RedirectToAction("Index", "Courses");
            }
           
        }
    }
}
