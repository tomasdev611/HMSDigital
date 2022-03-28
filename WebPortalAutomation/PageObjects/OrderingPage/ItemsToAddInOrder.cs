using System;
using OpenQA.Selenium;

using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public class ItemToAdd
    {
        public string name { get; }
        public string SKU { get; }
        public string id { get; }
        public int amount { get; } 

        public ItemToAdd(string name, int amount, string SKU = null, string id = null)
        {
            this.name = name;
            this.SKU = SKU;
            this.id = id;
            this.amount = amount;
        }
    }
}
