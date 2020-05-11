﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEWorkshop.ServiceLayer;
using SEWorkshop.DataModels;

namespace Website.Pages
{
    public class SearchResultModel : PageModel
    {
        private IUserManager UserManager { get; }
        public string RequestedSearchString { get; private set; }
        public string SearchCategory { get; private set; }
        public string ActualSearchString { get; private set; }
        public int? PriceMin { get; private set; }
        public int? PriceMax { get; private set; }
        public string? CategoryFilter { get; private set; }
        public string Error { get; private set; }
        public IEnumerable<DataProduct> SearchResult { get; private set; }

        public SearchResultModel(IUserManager userManager)
        {
            UserManager = userManager;
            SearchResult = new List<DataProduct>();
            RequestedSearchString = "";
            ActualSearchString = "";
            SearchCategory = "";
            Error = "";
        }

        public void OnPost(string searchText, string searchCategory, string minPriceText, string maxPriceText, string categoryFilterText)
        {
            if (!string.IsNullOrEmpty(categoryFilterText))
            {
                CategoryFilter = categoryFilterText;
            }
            try
            {
                if (!string.IsNullOrEmpty(minPriceText))
                {
                    PriceMin = int.Parse(minPriceText);
                }
                if (!string.IsNullOrEmpty(maxPriceText))
                {
                    PriceMax = int.Parse(maxPriceText);
                }
            }
            catch
            {
                Error = "Min price or max price contained non numbers";
            }


            RequestedSearchString = searchText;
            SearchCategory = searchCategory;
            string searchString = RequestedSearchString;
            SearchResult = SearchCategory switch
            {
                "product_name" => UserManager.SearchProductsByName(ref searchString),
                "product_category" => UserManager.SearchProductsByCategory(ref searchString),
                "product_keywords" => UserManager.SearchProductsByKeywords(ref searchString),
                _ => new List<DataProduct>()
            };
            ActualSearchString = searchString;
            FilterResults();
        }

        private void FilterResults()
        {
            if(PriceMax != null)
            {
                SearchResult = UserManager.FilterProducts(SearchResult.ToList(), prod => prod.Price <= PriceMax);
            }
            if (PriceMin != null)
            {
                SearchResult = UserManager.FilterProducts(SearchResult.ToList(), prod => prod.Price >= PriceMin);
            }
            if (CategoryFilter != null)
            {
                SearchResult = UserManager.FilterProducts(SearchResult.ToList(), prod => prod.Category.Equals(CategoryFilter));
            }
        }
    }
}