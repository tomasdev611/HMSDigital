using System;
using OpenQA.Selenium;

using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "")
        {
            this.elements = Utils.LoadJson(jsonFileName: "LoginPageElements.json");
            this.userTestCredentials = GetTestUsersCredentials();
        }

        public string GetUserData(string userDataToObtain)
        {
            return userTestCredentials[userDataToObtain.Replace(" ", "_").ToUpper()];
        }

        private Dictionary<string, string> GetTestUsersCredentials()
        {
            Dictionary<string, string> credentials = new Dictionary<string, string>();

            List<string> userTypes = new List<string>()
                {
                    "SITE_MANAGER", "DRIVER", "CUSTOM_SERVICE_REP",
                    "CUSTOM_SERVICE_SUPV", "HOSPICE_ADMIN", "HOSPICE_USER"
                };

            List<string> credentialType = new List<string>() { "EMAIL", "PASSWORD" };

            foreach(string user in userTypes)
            {
                foreach(string credential in credentialType)
                {
                    string credentialKey = user + "_" + credential;
                    credentials[credentialKey] = Environment.GetEnvironmentVariable("HMSDIGITAL_" + credentialKey);
                }
            }

            return credentials;
        }
    }
}
