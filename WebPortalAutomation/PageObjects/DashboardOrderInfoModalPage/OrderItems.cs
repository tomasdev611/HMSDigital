using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class DashboardOrderInfoModalPage
    {
        public OrderItemList GetOrderItemListDashboard()
        {
            List<IWebElement> items = FindManyWithRetries(element: "Items Ordered");

            List<OrderItem> orderItems = new List<OrderItem>();  
            foreach(IWebElement item in items)
            {
                string name = LoadContentFromElement("Item Name", "waitables", relativeTo: item);
                string amount = LoadContentFromElement("Item Amount", "waitables", relativeTo: item);
                string equipmentName = LoadContentFromElement("Equipment Name", "waitables", relativeTo: item);
                int? equipmentAmount = null;

                if(equipmentName is not null)
                {
                    string strEquAmount = LoadContentFromElement("Equipment Amount", "waitables", relativeTo: item);
                    equipmentAmount = Int32.Parse(strEquAmount);
                }

                orderItems.Add(new OrderItem(name, Int32.Parse(amount), equipmentName, equipmentAmount));
            }

            return new OrderItemList(orderItems);
        }

    }

    public class OrderItemList
    {
        List<OrderItem> Items;

        public OrderItemList(List<OrderItem> items)
        {
            this.Items = items;
        }

        public int NumberOfItems()
        {
            return this.Items.Count;
        }

        public OrderItem this[int index]
        {
            get
            {
                if( index < 0 || this.Items.Count <= index)
                {
                    throw new Exception($"Index value must be between 0 and {this.Items.Count - 1}");
                }

                return this.Items[index];
            }
        }
    }

    public class OrderItem
    {
        public string Name { get; }
        public int Amount { get; }
        public string EquipmentName { get; }
        public int? EquipmentAmount { get; }

        public OrderItem(
            string name,
            int amount,
            string equipmentName = null,
            int? equipmentAmount = null
        )
        {
            this.Name = name;
            this.Amount = amount;
            this.EquipmentName = equipmentName;
            this.EquipmentAmount = equipmentAmount;
        }

        public bool HasEquipment() { return this.EquipmentName is not null; }
    }

}
