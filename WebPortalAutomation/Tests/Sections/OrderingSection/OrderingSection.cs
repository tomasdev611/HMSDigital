using System;
using System.Threading;
using OpenQA.Selenium;

using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public class OrderingSection : OrderingPage
    {
        public OrderingSection(IWebDriver webdriver) : base(webdriver: webdriver) {}

        public AsserterItemsAddedInOrder SetItemsInOrder(List<ItemToAdd> items)
        {
            SwitchToIFrame("Inside IFrame");
            WaitElementLoad("Interactable section", retries: 10, waitElementTimeout: 5);
            
            AddItemsToTheCart(items);

            return GetItemsAddedInOrder();
        }

        public void ExitIFrameAndGoToDeliveryDetails()
        {
            Click("Next");
            ReturnToDefaultIFrame();
        }
    }
}
