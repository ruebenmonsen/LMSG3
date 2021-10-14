using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LMSG3.Web.Areas.Identity.Pages.Account
{
    // [Authorize(Roles = "Teacher")]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            ILogger<RegisterModel> logger)
        {

            _userManager = userManager;
            _logger = logger;
 
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

         
            [Display(Name = "Course")]
            public int? CourseId { get; set; }

            [Required]
            [Display(Name = "User Role")]
            public string UserRole { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            { 
                var Role = Input.UserRole;
                var user= new ApplicationUser();
                if (Role == "Teacher")
                {
                    user = new ApplicationUser
                    {
                        FName = Input.FName,
                        LName = Input.LName,
                        UserName = Input.Email,
                        Email = Input.Email

                    };
                }
                else if(Role == "Student")
                {
                    user = new Student
                    {
                        FName = Input.FName,
                        LName = Input.LName,
                        UserName = Input.Email,
                        Email = Input.Email,
                        CourseId= (int)Input.CourseId
                    };
                }
       
                var result = await _userManager.CreateAsync(user, Input.Password);
                var addToRoleResult = await _userManager.AddToRoleAsync(user, Role);


                if (result.Succeeded && addToRoleResult.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                     return RedirectToAction(nameof(Index),"Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
