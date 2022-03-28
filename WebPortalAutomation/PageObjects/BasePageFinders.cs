using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Threading; // Thread sleep defined here
using System.Collections.Generic; // Dictionary type defined here
using System.Collections.ObjectModel; // ReadOnlyCollection

namespace WebPortalAutomation
{
    public partial class BasePage
    {

        public List<IWebElement> FindManyWithRetries(
            By locator = null,
            string element = null,
            string parameterizedCategory = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info("Find many elements");

            if(locator is null)
            {
                if(element is null)
                {
                    throw new Exception("The locator and the element can't be null at the same time");
                }

                locator = GetSimpleLocatorFromJson(
                    element,
                    "waitables",
                    parameterizedCategory,
                    (relativeTo is not null)
                );
            }

            WebDriverWait wait = new WebDriverWait(
                webdriver,
                TimeSpan.FromSeconds(waitElementTimeout)
            );

            // This retry structure is repeated from FindAndOperateWithRetries because in that method
            // a singe iwebelement is returned, here I need a List and there is no time to
            // make polymorfism.
            for(int i = 0; i < retries - 1; i++)
            {
                try
                {
                    List<IWebElement> elementsFound = new List<IWebElement>(
                        wait.Until(FinderMultipleDefault(locator, relativeTo))
                    );
                    Logger.Info("Many elements have been found");
                    return elementsFound;
                }
                catch(Exception)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(sleep));   
                }
            }

            List<IWebElement> elemsFound = new List<IWebElement>(
                wait.Until(FinderMultipleDefault(locator, relativeTo))
            );
            Logger.Info("Many elements have been found");
            return elemsFound;
        }   

        // Private methods

        // This method tries to find the element until timeout is reached
        private IWebElement FindAndOperateWithRetries(
            Func<Dictionary<string, object>, IWebElement> method,
            Dictionary<string, object> args
        )
        {
            int retries = (int)args["retries"];
            int sleep = (int)args["sleep"];

            // Catch up to retries - 1 exceptions
            for(int i = 0; i < retries - 1; i++)
            {
                try
                {
                    return method(args);
                }
                catch(Exception)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(sleep));  
                }
            }

            // Try one more time (so the max amount of attemps is "retries")
            // If it fails, the exception will raise
            return method(args);
        }

        private IWebElement FindSingleWithRetries(
            By locator,
            IWebElement relativeTo = null,
            Func<IWebElement, Func<IWebDriver, IWebElement>> waiter = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Func<IWebDriver, IWebElement> finder = FinderDefault(locator, relativeTo);

            return FindAndOperateWithRetries(
                method: (args => FindAndWait(args, finder: finder, waiter: waiter)),
                args: new Dictionary<string, object>() {
                    // args used inside "FindAndWait"
                    { "waitElementTimeout", waitElementTimeout },
                    // args used inside "FindAndOperateWithRetries"
                    { "retries", retries },
                    { "sleep", sleep }
                }
            );
        }

        private IWebElement FindAndWait(
            Dictionary<string, object> args,
            Func<IWebDriver, IWebElement> finder = null,
            Func<IWebElement, Func<IWebDriver, IWebElement>> waiter = null
        )
        {
            // unpack args from dictionary
            int waitElementTimeout = (int)args["waitElementTimeout"];

            WebDriverWait wait = new WebDriverWait(
                webdriver,
                TimeSpan.FromSeconds(waitElementTimeout)
            );

            IWebElement element = wait.Until(finder);

            if(waiter is not null)
            {
                element = wait.Until(waiter(element));
            }
            return element;
        }

        private Func<IWebDriver, IWebElement> FinderDefault(
            By locator,
            IWebElement relativeTo
        )
        {
            // The driver would be the webdriver of the base page when the lambda is called
            return ( driver => (relativeTo is null) ? driver.FindElement(locator) : relativeTo.FindElement(locator));
        }

        private Func<IWebDriver, ReadOnlyCollection<IWebElement>> FinderMultipleDefault(
            By locator,
            IWebElement relativeTo
        )
        {
            return ( driver => (relativeTo is null) ? driver.FindElements(locator) : relativeTo.FindElements(locator));
        }

        private Func<IWebElement, Func<IWebDriver, IWebElement>> ElementIsVisible()
        {
            return (
                element => ( _ => element.Displayed ?
                    element : throw new Exception($"Element {element} is not visible")
                )
            );
        }
    }
}
