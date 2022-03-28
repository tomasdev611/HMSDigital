using System;
using OpenQA.Selenium;

using NUnit.Framework;

namespace WebPortalAutomation
{
    public class PatientInfoModalPage : BasePage
    {
        public PatientInfoModalPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "/patients")
        {
            this.elements = Utils.LoadJson(jsonFileName: "PatientInfoModalPageElements.json");
        }
    }
}
