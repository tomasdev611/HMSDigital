using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class MenuPage : BasePage
    {
        public MenuPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "")
        {
            this.elements = Utils.LoadJson(jsonFileName: "MenuPageElements.json");
        }
    }

    public class AsserterAccount
    {
        public string Name { get; }

        public AsserterAccount(string name)
        {
            this.Name = name;
        }
    }
}