using OpenQA.Selenium;
using System.Collections.Generic; // Dictionary type defined here
using System.Collections.ObjectModel; // ReadOnlyCollection

namespace WebPortalAutomation
{
    public class ReviewYourOrderPage : BasePage
    {
        public ReviewYourOrderPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "/orders/sca/delivery") 
        {
            this.elements = Utils.LoadJson(jsonFileName: "ReviewYourOrderPageElements.json");
        }

        protected object AtributesOfTableRowItemsOrdered(IWebElement tableRow)
        {
            // Get IWebElement attributes
            List<IWebElement> attributes = FindManyWithRetries(
                element: "Table Data",
                relativeTo: tableRow
            );

            // Get text contained in IWebElements
            List<string> values = new List<string>();
            
            int length = attributes.Count;
            for(int i = 0; i < length; i++)
            {
                values.Add(GetContent(attributes[i]));
            }

            return (object)(
                new Dictionary<string, string>(){
                    { "Name", values[0] },
                    { "SKU", values[1] },
                    { "Quantity", values[2] },
                    { "Price", values[3] },
                    { "Approval Required", values[4] },
                    { "Approver", values[5] }
                }
            );
        }
    }

    public class AsserterOrderCreated
    {

        public string PatientName { get; }
        public string Nurse { get; }
        public string DeliveryTiming { get; }
        public string OrderNotes { get; }
        public StaticTable ItemsTable { get; }

        public AsserterOrderCreated(
            string patientName,
            string nurse,
            string deliveryTiming,
            string orderNotes,
            StaticTable itemsTable
        )
        {
            this.PatientName = patientName;
            this.Nurse = nurse;
            this.DeliveryTiming = deliveryTiming;
            this.OrderNotes = orderNotes;
            this.ItemsTable = itemsTable;
        }
    }
}
