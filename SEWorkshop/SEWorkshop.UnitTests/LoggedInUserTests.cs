﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SEWorkshop.Adapters;
using SEWorkshop.Exceptions;
using SEWorkshop.Models;
using System.Linq;

namespace SEWorkshop.Tests
{
    [TestFixture]
    public class LoggedInUserTests
    {
        SecurityAdapter _securityAdapter = new SecurityAdapter();

        [Test]
        public void WriteReview()
        {

        }

        [Test]
        public void WriteMessage()
        {

        }

        [Test]
        public void AddProductTest()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            Product product = usr.AddProduct(store, "BestApp", "Authentic One", "App", 4.00, 10);

            Assert.IsTrue(store.Products.Contains(product));
            bool catched = false;
            try
            {
                usr.AddProduct(store, "BestApp", "Fake One", "App", 4.00, 10);
            }
            catch
            {
                catched = true;
            }
            Assert.IsTrue(catched);

        }

        [Test]
        public void RemoveProduct()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            Product product = usr.AddProduct(store, "BestApp", "Authentic One", "App", 4.00, 10);
            usr.RemoveProduct(store, product);
            Assert.IsTrue(!store.Products.Contains(product));
            bool catched = false;
            try
            {
                usr.RemoveProduct(store, product);
            }
            catch
            {
                catched = true;
            }
            Assert.IsTrue(catched);
        }

        [Test]
        public void EditProductDescription()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            Product product = new Product(store, "Whatsapp", "Great app!", "Personal Communication", 4.00, 100);
            store.Products.Add(product);
            usr.EditProductDescription(store, product, "Awesome App");
            Assert.IsTrue(product.Description.Equals("Awesome App"));

        }

        [Test]
        public void EditProductCategory()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            Product product = new Product(store, "Whatsapp", "Great app!", "Personal Communication", 4.00, 100);
            store.Products.Add(product);
            usr.EditProductCategory(store, product, "General Communication");
            Assert.IsTrue(product.Category.Equals("General Communication"));

        }

        [Test]
        public void EditProductName()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            Product product1 = new Product(store, "Whatsapp", "Great app!", "Personal Communication", 4.00, 100);
            Product product2 = new Product(store, "Instagram", "Great app!", "Personal Communication", 4.00, 100);
            store.Products.Add(product1);
            store.Products.Add(product2);
            usr.EditProductName(store, product1, "Messenger");
            Assert.IsTrue(product1.Name.Equals("Messenger"));
            bool catched = false;
            try
            {
                usr.EditProductName(store, product1, "Instagram");
            }
            catch
            {
                catched = true;
            }
            Assert.IsTrue(catched && product1.Name.Equals("Messenger"));

        }

        [Test]
        public void EditProductPrice()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            Product product = new Product(store, "Whatsapp", "Great app!", "Personal Communication", 4.00, 100);
            store.Products.Add(product);
            usr.EditProductPrice(store, product, 0.00);
            Assert.IsTrue(product.Price == 0);


        }

        [Test]
        public void EditProductQuantity()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            Product product = new Product(store, "Whatsapp", "Great app!", "Personal Communication", 4.00, 100);
            store.Products.Add(product);
            usr.EditProductQuantity(store, product, 200);
            Assert.IsTrue(product.Quantity == 200);

        }
        [Test]
        public void SetPermissionsOfManagerTestOwner()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newManager = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreManager(store, newManager);
            usr.SetPermissionsOfManager(store, newManager, Authorizations.Products);


        }

        [Test]
        public void SetPermissionsOfManagerTestManager()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newManager = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreManager(store, newManager);
            usr.SetPermissionsOfManager(store, newManager, Authorizations.Manager);
            Assert.IsTrue(store.Managers[newManager] == newManager);
        }


        [Test]
        public void SetPermissionsOfManagerTestReplying()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newManager = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreManager(store, newManager);

            usr.SetPermissionsOfManager(store, newManager, Authorizations.Replying);
            LoggedInUser client = new LoggedInUser("client1", _securityAdapter.Encrypt("1234"));
            Message message1 = new Message(client, "Great store!");
            store.Messages.Add(message1);
            Message reply1 = newManager.MessageReply(message1, store, "Thank you!");

            Assert.IsTrue(message1.Next == reply1 && reply1.Prev == message1
                            && reply1.Description.Equals("Thank you!"));
        }

        [Test]
        public void SetPermissionsOfManagerTestWatching()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newManager = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreManager(store, newManager);
            usr.SetPermissionsOfManager(store, newManager, Authorizations.Watching);
            LoggedInUser client = new LoggedInUser("client1", _securityAdapter.Encrypt("1234"));
            Message message1 = new Message(client, "Great store!");
            store.Messages.Add(message1);
            IEnumerable<Message> output1 = newManager.getMessage(store);
            Assert.IsTrue(output1.Contains(message1));

        }

        [Test]
        public void SetPermissionsOfManagerTestProducts()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newManager = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreManager(store, newManager);
            usr.SetPermissionsOfManager(store, newManager, Authorizations.Products);
            Product product;
            product = newManager.AddProduct(store, "ManagerProduct1", "Facebook", "Social", 4.00, 100);
            Assert.IsTrue(store.Products.Contains(product));

        }

        [Test]
        public void AddStoreOwnerTest()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newOwner = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreOwner(store, newOwner);
            Assert.IsTrue(store.Owners.Contains(new KeyValuePair<LoggedInUser, LoggedInUser>(newOwner, usr)));

        }



        [Test]
        public void AddStoreManagerTest()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newManager = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreManager(store, newManager);
            Assert.IsTrue(store.Managers.Contains(new KeyValuePair<LoggedInUser, LoggedInUser>(newManager, usr)));
            bool catched = false;
            try
            {
                usr.AddStoreManager(store, newManager);
            }
            catch
            {
                catched = true;
            }
            Assert.IsTrue(catched);

        }


        [Test]
        public void RemoveStoreManager()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser newManager = new LoggedInUser("appmanager1", _securityAdapter.Encrypt("1234"));
            usr.AddStoreManager(store, newManager);
            usr.RemoveStoreManager(store, newManager);
            Assert.IsTrue(!store.Managers.Contains(new KeyValuePair<LoggedInUser, LoggedInUser>(newManager, usr)));
            bool catched = false;
            try
            {
                usr.RemoveStoreManager(store, newManager);
            }
            catch
            {
                catched = true;
            }
            Assert.IsTrue(catched);
        }

        [Test]
        public void MessageReply()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser client = new LoggedInUser("client", _securityAdapter.Encrypt("1324"));
            Message message = new Message(client, "Great app");
            store.Messages.Add(message);
            Message reply = usr.MessageReply(message, store, "Thank you!");
            Assert.IsTrue(reply.Prev == message && message.Next == reply && reply.Description.Equals("Thank you!"));

        }

        [Test]
        public void getMassage()
        {
            const string STORE_NAME = "Google Play";

            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));

            Store store = new Store(usr, STORE_NAME);
            LoggedInUser client = new LoggedInUser("client", _securityAdapter.Encrypt("1324"));

            Message message = new Message(client, "Great app");
            store.Messages.Add(message);

            IEnumerable<Message> messages = usr.getMessage(store);
            /*
              Assert.IsTrue(messages.Count() == 1 && messages.First() == message);*/

        }

        [Test]
        public void ViewPurchaseHistory()
        {
            const string STORE_NAME = "Google Play";
            LoggedInUser usr = new LoggedInUser("appdevloper1", _securityAdapter.Encrypt("1234"));
            Store store = new Store(usr, STORE_NAME);
            LoggedInUser client = new LoggedInUser("client", _securityAdapter.Encrypt("1324"));
            Purchase purchase = new Purchase(client, new Basket(store));
            store.Purchases.Add(purchase);
            IEnumerable<Purchase> purchases = usr.ViewPurchaseHistory(store);
            Assert.IsTrue(purchases.Count() == 1 && purchases.First() == purchase);

        }

    }
}
