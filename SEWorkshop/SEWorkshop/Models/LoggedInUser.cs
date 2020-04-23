﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SEWorkshop.Models
{
    public class LoggedInUser : User
    {
        public ICollection<Store> Owns { get; private set; }
        public ICollection<Store> Manages { get; private set; }
        public IList<Review> Reviews { get; private set; }
        public IList<Message> Messages { get; private set; }
        public IList<Purchase> PurchaseHistory{get; private set;}
        public string Username {get; private set;}
        public byte[] Password {get; private set;}   //it will be SHA256 encrypted password

        public LoggedInUser(string username, byte[] password)
        {
            Username = username;
            Password = password;
            Owns = new List<Store>();
            Manages = new List<Store>();
            Reviews = new List<Review>();
            Messages = new List<Message>();
            PurchaseHistory= new List<Purchase>();
        }
    }
}