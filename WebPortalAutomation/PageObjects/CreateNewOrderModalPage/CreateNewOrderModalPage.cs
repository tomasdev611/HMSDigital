using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class CreateNewOrderModalPage : BasePage
    {
        public CreateNewOrderModalPage(IWebDriver webdriver) : 
            // Is a modal so it has not a specific url
            base(webdriver: webdriver, pathURL: "") 
        {
            this.elements = Utils.LoadJson(jsonFileName: "CreateNewOrderModalPageElements.json");
        }
    }
}
