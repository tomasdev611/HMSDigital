using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class DeliveryDetailsPage : BasePage
    {
        public DeliveryDetailsPage(IWebDriver webdriver) : 
            // // TODO: fix url
            base(webdriver: webdriver, pathURL: "/orders/sca/delivery") 
        {
            this.elements = Utils.LoadJson(jsonFileName: "DeliveryDetailsPageElements.json");
        }
    }
}
