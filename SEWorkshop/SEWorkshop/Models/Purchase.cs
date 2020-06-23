﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SEWorkshop.DAL;
using System.Linq;

namespace SEWorkshop.Models
{
    public class Purchase
    {
        public virtual Administrator Admin { get; set; }
        public virtual string AdminUserName { get; set; }
        public virtual string? Username { get; set; }
        public virtual LoggedInUser? User { get; private set;}
        public virtual int BasketId { get; set; }
        public virtual Basket Basket { get; private set; }
        public virtual int AddressId { get; set; }
        public virtual Address Address { get; set;  }
        public virtual DateTime TimeStamp { get; set;  }
        public virtual double MoneyPaid { get; set; }

        public virtual string Country { get; set; }
        public virtual string City { get; set; }
        public virtual string Street { get; set; }
        public virtual string HouseNumber { get; set; }
        public virtual string Zip { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. THIS IS FOR EF.
        public Purchase()
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
        {

        }


        public Purchase(LoggedInUser? user, Basket basket, Address adrs)
        {
            User = user;
            Basket = basket;
            TimeStamp = DateTime.Now;
            Address = adrs;
            MoneyPaid = basket.PriceAfterDiscount();
            Basket.Purchase = this;
            AdminUserName = Administrator.ADMIN_USER_NAME;
            Admin = DatabaseProxy.Instance.Administrators.FirstOrDefault();

            Country = adrs.Country;
            City = adrs.City;
            Street = adrs.Street;
            HouseNumber = adrs.HouseNumber;
            Zip = adrs.Zip;
        }
    }
}
