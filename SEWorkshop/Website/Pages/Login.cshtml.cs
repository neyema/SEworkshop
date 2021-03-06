﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEWorkshop.ServiceLayer;
using System;
using System.Linq;

namespace Website.Pages
{
    public class LoginModel : PageModel
    {
        public IUserManager UserManager { get; }

        [BindProperty (SupportsGet = true)]
        public string Username { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Error { get; set; }

        public LoginModel(IUserManager userManager)
        {
            UserManager = userManager;
            Username = "";
            Password = "";
            Error = "";
        }

        public void OnGet()
        {
            Error = "";
        }

        public IActionResult OnPost()
        {
            try
            {
                UserManager.Login(HttpContext.Session.Id, Username, Password);
            }
            catch (Exception)
            {
                //Error = e.ToString();
                Error = "Can't login";
                return new PageResult();
            }
            return RedirectToPage("./Stores");
        }
    }
}