﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SEWorkshop.Models.Policies
{
    public class UserCountryPolicy : Policy
    {
        public string RequiredCountry { get; set; }

        public UserCountryPolicy(Store store, string requiredCountry) : base(store)
        {
            RequiredCountry = requiredCountry;
        }

        protected override bool IsThisPolicySatisfied(User user, Address address)
        {
            return address.Country.Equals(RequiredCountry);
        }
    }
}