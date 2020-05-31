﻿using System;
using System.Collections.Generic;
using System.Text;
using SEWorkshop.Models.Discounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEWorkshop.Models
{
    [Table("Products")]
    public class Product
    {
        [ForeignKey("Stores"), Key, Column(Order = 0)]
        public Store Store { get; private set; }
        [Key, Column(Order = 1)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<Review> Reviews {get ; set;}

        public Product(Store store, string name, string description, string category, double price, int quantity)
        {
            Store = store;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
            Reviews = new List<Review>();
            Quantity = quantity;
        }

        public double PriceAfterDiscount()
        {
            double price = Price;
            ICollection<(Product, int)> product = new List<(Product, int)>{(this, 1)};
            foreach (var discount in Store.Discounts)
            {
                if (discount.InnerDiscount is null && discount is OpenDiscount)
                {
                    if (((OpenDiscount) discount).Product == this)
                    {
                        price -= discount.ComposeDiscounts(product);
                    }
                }
            }

            return price;
        }

        public override string ToString()
        {
            string output = "Name: " + Name +
            "\nDescription: " + Description + 
            "\nStore: " + Store.Name +
            "\nCategory: " + Category +
            "\nPrice: " + Price;
            return output;
        }
    }
}
