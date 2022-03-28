using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class LoginSection : LoginPage
    {
        public LoginSection(IWebDriver webdriver) : base(webdriver: webdriver) {}

        public void LoginToWebPortal()
        {
            WaitElementLoad("Interactable section");
            SendInput(element: "Sign In Username", input: GetUserData("Hospice Admin email"));
            SendInput(element: "Sign In Password", input: GetUserData("Hospice Admin password"));
            Click("Sign In Submit");
        }
    }
}
