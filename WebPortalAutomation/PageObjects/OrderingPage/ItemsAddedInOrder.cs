using System;
using OpenQA.Selenium;

using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class OrderingPage
    {
        public AsserterItemsAddedInOrder GetItemsAddedInOrder()
        {
            Logger.Info("Obtaining items added in cart");

            Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>();
            Dictionary<string, ItemAdded> itemsInCart = new Dictionary<string, ItemAdded>();
            
            List<IWebElement> items = FindManyWithRetries(element: "Items added to cart");

            Logger.Info($"AMOUNT OF ITEMS ADDED: {items.Count}");

            foreach(IWebElement item in items)
            {
                string name = LoadContentFromElement(
                    element: "Item added name",
                    type: "waitables",
                    relativeTo: item
                );

                string category = LoadContentFromElement(
                    element: "Item added category",
                    type: "waitables",
                    relativeTo: item
                );

                int amount = int.Parse(LoadAttributeFromElement(
                    element: "Item added amount",
                    type: "writables",
                    relativeTo: item,
                    attribute: "value"
                ));

                itemsInCart[name] = new ItemAdded(name, category, amount);

                if(categories.ContainsKey(category))
                {
                    categories[category].Add(name);
                }
                else
                {
                    categories[category] = new List<string>() { name };
                }
            }
            
            AsserterItemsAddedInOrder asserter = new AsserterItemsAddedInOrder(categories, itemsInCart); 
            Logger.Info("Items added in cart obtained");
            return asserter;
        }
    }

    public class AsserterItemsAddedInOrder
    {
        Dictionary<string, List<string>> categories;
        Dictionary<string, ItemAdded> itemsInCart;

        public AsserterItemsAddedInOrder(
            Dictionary<string, List<string>> categories,
            Dictionary<string, ItemAdded> itemsInCart
        )
        {
            this.categories = categories;
            this.itemsInCart = itemsInCart;  
        }

        public ItemAdded GetItem(string name)
        {
            if(!this.itemsInCart.ContainsKey(name))
            {
                throw new Exception($"Invalid name ({name}) of Item Added.");
            }
            return this.itemsInCart[name];
        }

        public List<ItemAdded> ItemsByCategory(string category)
        {
            if(!this.categories.ContainsKey(category))
            {
                throw new Exception($"Invalid category ({category})");
            }
            
            List<string> categ = this.categories[category];
            List<ItemAdded> resultItems = new List<ItemAdded>();

            foreach(string itemName in categ)
            {
                resultItems.Add(this.itemsInCart[itemName]);
            }

            return resultItems;
        }

        public int NumberOfItemsAdded()
        {
            return this.itemsInCart.Count;
        }
    }

    public class ItemAdded
    {
        public string name { get; }
        public string category { get; }
        public int amount { get; }

        public ItemAdded(string name, string category, int amount)
        {
            this.name = name;
            this.category = category;
            this.amount = amount;
        }
    }
}
