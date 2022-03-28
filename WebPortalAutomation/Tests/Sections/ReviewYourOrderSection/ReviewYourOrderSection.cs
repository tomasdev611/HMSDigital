using OpenQA.Selenium;

using System.Threading;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public class ReviewYourOrderSection : ReviewYourOrderPage
    {
        public ReviewYourOrderSection(IWebDriver webdriver) : base(webdriver: webdriver) {}

        public AsserterOrderCreated GetOrderElementsToAssert()
        {
            SwitchToIFrame("Inside IFrame");

            // TODO check if more data is needed
            string patientName = LoadContentFromElement("Patient Name", "waitables").Split(":")[1];
            string nurse = LoadContentFromElement("Ordering Nurse", "waitables").Split(":")[1];
            string deliveryTiming = LoadContentFromElement("Delivery Timing", "waitables");
            string orderNotes = LoadContentFromElement("Order Notes", "waitables");
            StaticTable itemsOrdered = GetTable(
                elementHeaders: "Items Table - Headers",
                elementRows: "Items Table - Rows"
            );

            ReturnToDefaultIFrame();

            return new AsserterOrderCreated(
                patientName, nurse, deliveryTiming, orderNotes, itemsOrdered
            );
        }

        public void GoToPlaceOrder()
        {
            SwitchToIFrame("Inside IFrame");
            Click("Place Order");
            ReturnToDefaultIFrame();
        }
    }
}
