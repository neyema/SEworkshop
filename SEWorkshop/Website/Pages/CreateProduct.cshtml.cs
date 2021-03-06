﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEWorkshop.ServiceLayer;
using SEWorkshop.DataModels;
using SEWorkshop.Exceptions;
using System.Security.Cryptography;

namespace Website.Pages
{

    public class CreateProductModel : PageModel
    {
        public IUserManager UserManager { get; }
        public DataProduct? Product { get; private set; }
        public string ProductName { get; private set; }
        public string ProductCategory { get; private set; }
        public string ProductDescription { get; private set; }
        public string ProductPrice { get; private set; }
        public string ProductQuantity { get; private set; }
        public string StoreName { get; private set; }
        public string ErrorMsg { get; private set; }

        public CreateProductModel(IUserManager userManager) {
            UserManager = userManager;
            ProductName = "";
            StoreName = "";
            ProductCategory = "";
            ProductDescription = "";
            ProductPrice = "";
            ProductQuantity = "";
            ErrorMsg = "";

        }
        public void OnGet()
        {
            ErrorMsg = "";
        }
        public IActionResult OnPost(string ProductName, string StoreName,  string ProductDescription, string ProductCategory, string ProductPrice, string ProductQuantity)
        {
            double price;
            int quantity;
            try
            {
                price = double.Parse(ProductPrice);
                quantity = int.Parse(ProductQuantity);
            }
            catch(Exception)
            {
                ErrorMsg = "Invalid Product Input";
                return new PageResult();
            }
            if(StoreName == null || ProductName == null || ProductDescription == null ||
            ProductCategory == null || price <= 0  || quantity <= 0)
            {
                ErrorMsg = "Invalid Product Input";
                return new PageResult();
            }
            
            try
            {
                UserManager.AddProduct(HttpContext.Session.Id, StoreName, ProductName, ProductDescription, ProductCategory, price, quantity);
            }
            catch (UserHasNoPermissionException e)
            {
                ErrorMsg = e.ToString();
                return new PageResult();
            }
            catch (ProductAlreadyExistException e)
            {
                ErrorMsg = e.ToString();
                return new PageResult();
            }
            catch (NegativeQuantityException e)
            {
                ErrorMsg = e.ToString();
                return new PageResult();
            }
            catch (StoreNotInTradingSystemException e)
            {
                ErrorMsg = e.ToString();
                return new PageResult();
            }
         
            this.ProductName = ProductName;
            this.StoreName = StoreName;
            this.ProductCategory = ProductCategory;
            this.ProductDescription = ProductDescription;
            this.ProductPrice = ProductPrice;
            this.ProductQuantity = ProductQuantity;

            return RedirectToPage("./Store", new {storeName= StoreName});

        }

    }
}