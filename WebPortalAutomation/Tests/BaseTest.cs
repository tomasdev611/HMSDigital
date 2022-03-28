using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public partial class BaseTest
    {
        // This TestContext is to be able to get the parameters passed when calling the dotnet test command
        //public TestContext TestContext { get; set; }
        protected IWebDriver webdriver { get; private set; }
        protected string browser { get; private set; }

        private bool headless;
        private string environmentURL = SetEnvironmentToTest();
        
        [SetUp]
        protected void SetUp()
        {   
            Logger.Info($"========== Test Starting ==========");
            // Start webdriver
            webdriver = SetBrowser();
            // Go to environment
            webdriver.Navigate().GoToUrl(environmentURL);
            // Maximize window
            if(!headless)
            {
                webdriver.Manage().Window.Maximize();
            }
      
            Logger.Info($"Browser configurated and running");
        }

        [TearDown]
        protected void TearDown()
        {
            Logger.Info("Finishing test");

            if(webdriver is not null)
            {
                CheckOutcome();
                webdriver.Quit();
                Logger.Info($"========== Test Finished: released resources ==========");
            }
        }

        public void AssertTrimEquals(string str1, string str2)
        {
            Assert.AreEqual(str1.Trim(), str2.Trim());
        }
    }
}
