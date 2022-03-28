using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class DeliveryDetailsSection : DeliveryDetailsPage
    {
        public DeliveryDetailsSection(IWebDriver webdriver) : base(webdriver: webdriver) {}

        public void HighPriority2hsHospiceUser()
        {
            SwitchToIFrame("Inside IFrame");

            Click("High Priority", retries: 8);
            Click("2 Hrs", parameterizedCategory: "Delivery Time");
            SendInput("Order Notes", input: "Automation");

            SelectOption("Ordering Nurse", option: "Paul Shea");
            Click("Continue");

            ReturnToDefaultIFrame();
        }
    }
}
