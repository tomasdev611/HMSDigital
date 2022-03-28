using System.Threading;

using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public partial class DashboardOrderInfoModalPage : BasePage
    {
        public DashboardOrderInfoModalPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "/dashboard")
        {
            this.elements = Utils.LoadJson(jsonFileName: "DashboardOrderInfoModalPageElements.json");
        }
    }
}
