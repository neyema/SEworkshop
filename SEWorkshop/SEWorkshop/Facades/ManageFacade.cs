using SEWorkshop.Models;
using SEWorkshop.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SEWorkshop.Facades
{
    public class ManageFacade : IManageFacade
    {
        private static ManageFacade? Instance { get; set; } = null;

        private ManageFacade()
        {
        }

        public static ManageFacade GetInstance()
        {
            if (Instance is null)
            {
                Instance = new ManageFacade();
            }
            return Instance;
        }

        public bool UserHasPermission(LoggedInUser loggedInUser, Store store)
        {
            ICollection<Authorizations>? authorizations;
            // to add a product it is required that the user who want to add the proudct is a store owner or a manager
            if ((isUserAStoreOwner(loggedInUser, store)
                || (isUserAStoreManager(loggedInUser, store))
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)// ravid explain this to me 
                    && authorizations != null           //user must be logged in to add a product
                    && authorizations.Contains(Authorizations.Products))) { return true; }
            else
            {
                return false;
            }

        }

        public void AddProduct(LoggedInUser loggedInUser, Store store, string name, string description, string category, double price)
        {
            // to add a product it is required that the user who want to add the proudct is a store owner or a manager
            if (UserHasPermission(loggedInUser, store)) 
            {
                Product newProduct = new Product(store, name, description, category, price);
                if (!store.Products.Contains(newProduct))
                {
                    store.Products.Add(newProduct);
                    return;
                }
            }
            throw new UserHasNoPermissionException();
        }

        public void RemoveProduct(LoggedInUser loggedInUser, Store store, Product product)
        {
            if (UserHasPermission(loggedInUser,store))
            {
                if (store.Products.Contains(product))
                {
                    store.Products.Remove(product);
                    return;
                }
            }
            throw new UserHasNoPermissionException();
        }

        public void AddStoreOwner(LoggedInUser loggedInUser, Store store, LoggedInUser newOwner)
        {
            ICollection<Authorizations>? authorizations;
            if((isUserAStoreOwner(loggedInUser, store)
                || (isUserAStoreManager(loggedInUser, store)
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)
                    && authorizations != null
                    && authorizations.Contains(Authorizations.Owner)))
                && !isUserAStoreOwner(newOwner,store))
            {
                store.Owners.Add(newOwner, loggedInUser);
                newOwner.Owns.Add(store);
                return;
            }
            throw new UserHasNoPermissionException();
        }

        public void AddStoreManager(LoggedInUser loggedInUser, Store store, LoggedInUser newManager)
        {
            ICollection<Authorizations>? authorizations;
            if ((isUserAStoreOwner(loggedInUser, store)
                    || (isUserAStoreManager(loggedInUser, store)
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)
                    && authorizations != null
                    && authorizations.Contains(Authorizations.Manager)))
                && !isUserAStoreOwner(newManager,store))
            {
                store.Managers.Add(newManager, loggedInUser);
                newManager.Manages.Add(store, new List<Authorizations>()
                {
                    Authorizations.Watching
                });
                return;
            }
            throw new UserHasNoPermissionException();
        }

        public void SetPermissionsOfManager(LoggedInUser loggedInUser, Store store, LoggedInUser manager, Authorizations authorization)
        {
            ICollection<Authorizations>? authorizations;
            if ((isUserAStoreOwner(loggedInUser, store)
                    || (isUserAStoreManager(loggedInUser, store)
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)
                    && authorizations != null
                    && authorizations.Contains(Authorizations.Authorizing)))
                && !isUserAStoreOwner(manager,store))
            {
                if(loggedInUser.Manages.TryGetValue(store, out authorizations)
                        && authorizations.Contains(authorization))
                {
                    authorizations.Remove(authorization);
                }
                else if (authorizations != null && authorizations.Contains(authorization))
                {
                    authorizations.Add(authorization);
                }
                return;
            }
            throw new UserHasNoPermissionException();
        }

        public void RemoveStoreManager(LoggedInUser loggedInUser, Store store, LoggedInUser managerToRemove)
        {
            ICollection<Authorizations>? authorizations;
            if ((isUserAStoreOwner(loggedInUser, store)
                    || (isUserAStoreManager(loggedInUser, store)
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)
                    && authorizations != null
                    && authorizations.Contains(Authorizations.Manager)))
                && isUserAStoreOwner(managerToRemove,store))
            {
                LoggedInUser? appointer;
                if(!store.Managers.TryGetValue(managerToRemove, out appointer) ||
                    appointer != loggedInUser)
                {
                    throw new UserHasNoPermissionException();
                }
                store.Managers.Remove(managerToRemove);
                managerToRemove.Manages.Remove(store);
            }
            throw new UserHasNoPermissionException();
        }

        public IEnumerable<Message> ViewMessage(LoggedInUser loggedInUser, Store store)
        {
            ICollection<Authorizations>? authorizations;
            if(isUserAStoreOwner(loggedInUser, store) ||
                (isUserAStoreManager(loggedInUser, store)
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)
                    && authorizations != null
                && authorizations.Contains(Authorizations.Replying)))
            {
                return store.Messages;
            }
            throw new UserHasNoPermissionException();
        }

        public void MessageReply(LoggedInUser loggedInUser, Message message, Store store, string description)
        {
            ICollection<Authorizations>? authorizations;
            if(isUserAStoreOwner(loggedInUser, store) ||
                (isUserAStoreManager(loggedInUser, store)
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)
                    && authorizations != null
                && authorizations.Contains(Authorizations.Replying)))
            {
                Message reply = new Message(loggedInUser, description, message);
                message.Next = reply;
            }
            throw new UserHasNoPermissionException();
        }

        public IEnumerable<Purchase> ViewPurchaseHistory(LoggedInUser loggedInUser, Store store)
        {
            ICollection<Authorizations>? authorizations;
            if(isUserAStoreOwner(loggedInUser, store) ||
                (isUserAStoreManager(loggedInUser, store)
                    && loggedInUser.Manages.TryGetValue(store, out authorizations)
                    && authorizations != null
                && authorizations.Contains(Authorizations.Replying)))
            {
                return store.Purchases;
            }
            throw new UserHasNoPermissionException();
        }

        bool isUserAStoreOwner(LoggedInUser user, Store store) => ((from owner in store.Owners
                                                                    where owner.Value == user
                                                                    select owner).ToList().Count()> 0);

        bool isUserAStoreManager(LoggedInUser user, Store store) => ((from manager in store.Managers
                                                                    where manager.Value == user
                                                                    select manager).ToList().Count() > 0);

        public void EditProductPrice(Product product, string description) => product.Description = description;

        public void EditProductCategory(Product product, string category) => product.Category = category;

        public void EditProductName(Product product, string name) => product.Name = name;

        public void EditProductPrice(Product product, double price) => product.Price = price;
    }
}