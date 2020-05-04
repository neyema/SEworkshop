﻿using NLog;
using SEWorkshop.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SEWorkshop.Models
{

    public abstract class AuthorityHandler
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public ICollection<Authorizations> AuthoriztionsOfUser { get; private set; }

        public abstract void AddStoreManager(LoggedInUser newManager);

        public abstract void RemoveStoreManager(LoggedInUser newManager);

        abstract public Product AddProduct(string name, string description, string category, double price, int quantity);

        abstract public void RemoveProduct(Product productToRemove);

        abstract public void EditProductDescription(Product product, string description);
        abstract public void EditProductCategory(Product product, string category);
        abstract public void EditProductName(Product product, string name);
        abstract public void EditProductPrice(Product product, double price);
        abstract public void EditProductQuantity(Product product, int quantity);
        
        public bool IsUserStoreOwner(LoggedInUser loggedInUser, Store store) => ((from owner in store.Owners
                                                                     where owner.Key == loggedInUser
                                                                     select owner).ToList().Count() > 0);
      
        public bool IsUserStoreManager(LoggedInUser loggedInUser, Store store) => ((from manager in store.Managers
                                                                       where manager.Key == loggedInUser
                                                                       select manager).ToList().Count() > 0);
        public bool StoreContainsProduct(Product product, Store store) => ((from pr in store.Products
                                                               where pr.Name == product.Name
                                                               select product).ToList().Count() > 0);

        public bool UserHasPermission(Store store ,LoggedInUser loggedInUser, Authorizations authorization)
        {
            // to add a product it is required that the user who want to add the proudct is a store owner or a manager
            return (IsUserStoreOwner(loggedInUser, store)
                    || (IsUserStoreManager(loggedInUser, store)
                        && AuthoriztionsOfUser.Contains(authorization)));
        }
     public IEnumerable<Purchase> ViewPurchaseHistory(LoggedInUser loggedInUser, Store store)
        {
            log.Info("User tries to view purchase history of store {0}", store.Name);
            if (UserHasPermission(store, loggedInUser ,Authorizations.Watching))
            {
                log.Info("Data has been fetched successfully");
                return store.Purchases;
            }
            log.Info("User has no permission for that action");
            throw new UserHasNoPermissionException();

        }

        public IEnumerable<Message> GetMessage(Store store, LoggedInUser loggedInUser)
        {
            log.Info("User tries to view messages of store {0}", store.Name);
            if (UserHasPermission(store, loggedInUser, Authorizations.Watching))
            {
                log.Info("Data has been fetched successfully");
                return store.Messages;
            }
            log.Info("User has no permission for that action");
            throw new UserHasNoPermissionException();
        }

        public Message MessageReply(LoggedInUser loggedInUser, Store store, Message message, string description)
        {
            log.Info("User tries to reply to a message");
            if (UserHasPermission(store, loggedInUser, Authorizations.Replying))
            {
                Message reply = new Message(loggedInUser, description, message);
                message.Next = reply;
                log.Info("Reply has been published successfully");
                return reply;
            }
            log.Info("User has no permission for that action");
            throw new UserHasNoPermissionException();
        }



    }
}
