﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEWorkshop.ServiceLayer;
using SEWorkshop.DataModels;
using SEWorkshop.Exceptions;

namespace Website.Pages
{
    public class StoreModel : PageModel
    {
        public IUserManager UserManager { get; }
        public DataStore? Store { get; private set; }
        public string StoreName {get; private set; }
        public string ErrorMsg { get; private set; }
        public DataLoggedInUser LoggedUser { get; private set; }
        public StoreModel(IUserManager userManager)
        {
            UserManager = userManager;
            StoreName = "";
            ErrorMsg = "";
        }
        
        public void OnGet(string storeName)
        {
            LoggedUser = UserManager.GetDataLoggedInUser(HttpContext.Session.Id);
            try
            {
                StoreName = storeName;
                Store = UserManager.SearchStore(StoreName);
            }
            catch(Exception e)
            {
                ErrorMsg = e.ToString();
            }
        }

        public IActionResult OnPost(string storeName)
        {
            try
            {
                StoreName = storeName;
                Store = UserManager.SearchStore(StoreName);
            }
            catch (StoreNotInTradingSystemException e)
            {
                ErrorMsg = e.ToString();
            }
            return RedirectToPage("./Manage", new { StoreName = StoreName });
        }
    }
}