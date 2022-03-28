using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class MenuSection : MenuPage
    {
        public MenuSection(IWebDriver webdriver) : base(webdriver: webdriver) {}

        public AsserterAccount GetAccountName()
        {
            string AccName = LoadContentFromElement("Profile", type: "clickables");
        
            return new AsserterAccount(AccName);
        }

        public void GoToPatientPage()
        {
            WaitElementLoad("Blue Left Menu", retries: 10);
            Click("Patients", parameterizedCategory: "Menu sections");
        }
    }
}
