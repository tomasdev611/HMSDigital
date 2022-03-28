using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {
        // in seconds, timeout passed to selenium to wait until the
        // presence of the element, element to be clickable, etc
        protected const int WaitLoadElementTimeout = 4;

        // how many times the method will attemp to find an element and execute
        // another selenium methods
        protected const int NumberOfRetries = 4;

        // in seconds, time the thread will be sleeping in the scheduler
        // when the program is polling to find an element
        protected const int SleepThread = 1;

        protected IWebDriver webdriver { get; }
        protected string pathURL;
        protected Dictionary<string, object> elements;
        protected Dictionary<string, object> genericElements;
        protected Dictionary<string, string> userTestCredentials;

        public BasePage(IWebDriver webdriver, string pathURL)
        {
            this.webdriver = webdriver;
            this.pathURL = pathURL;
            this.genericElements = Utils.LoadJson("GenericWebElements.json");
        }

    }
}
