using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class CreateNewOrderModalSection : CreateNewOrderModalPage
    {
        public CreateNewOrderModalSection(IWebDriver webdriver) :
            base(webdriver: webdriver) {}

        public void SelectOrderToCreate(string type)
        {
            WaitElementLoad("Modal");
            Click(type, parameterizedCategory: "Order Type");
            Click("Next", parameterizedCategory: "Action Button");
        }
    }
}
