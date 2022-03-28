using System.Threading;

using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public partial class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "/dashboard")
        {
            this.elements = Utils.LoadJson(jsonFileName: "DashboardPageElements.json");
        }
    }
}
