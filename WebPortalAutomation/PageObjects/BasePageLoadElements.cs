using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {

        public IWebElement WaitElementLoad(
            string element,
            string parameterizedCategory = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info($"Waiting element --{element}-- to be loaded");

            By locator = GetSimpleLocatorFromJson(
                element: element,
                type: "waitables",
                category: parameterizedCategory,
                checkRelative: (relativeTo is not null)
            );

            IWebElement elementFounded = FindSingleWithRetries(
                locator,
                waiter: ElementIsVisible(),
                relativeTo: relativeTo,
                retries: retries,
                sleep: sleep,
                waitElementTimeout: waitElementTimeout
            );

            Logger.Info($"Element --{element}-- loaded");

            return elementFounded;
        }

        public string LoadContentFromElement(
            string element,
            string type,
            string parameterizedCategory = null,
            string option = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            return LoadAttributeFromElement(
                element,
                type,
                parameterizedCategory,
                null,
                option,
                relativeTo,
                waitElementTimeout,
                retries,
                sleep
            );
        }

        public string LoadAttributeFromElement(
            string element,
            string type,
            string parameterizedCategory = null,
            string attribute = null,
            string option = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            string load = (attribute is null) ? "content" : $"attribute {attribute}";
            Logger.Info($"Loading {load} from element --{element}-- of type --{type}--");

            By locator = GetGenericLocatorFromJson(
                element,
                type,
                option,
                parameterizedCategory,
                checkRelative: (relativeTo is not null)
            );

            IWebElement elem;

            try
            {
                elem = FindSingleWithRetries(
                    locator,
                    relativeTo: relativeTo,
                    waiter: ElementIsVisible(),
                    retries: retries,
                    sleep: sleep,
                    waitElementTimeout: waitElementTimeout
                );
            }
            catch(Exception e)
            {
                Logger.Error($"Exception raised finding the element: {e}");
                return null;
            }

            string attr = (attribute is null) ? elem.Text : elem.GetAttribute(attribute);
            Logger.Info($"Finish loading --{attr}-- from element --{element}--");
            return attr?.Trim();
        }

        public string GetContent(IWebElement element)
        {
            return element.Text?.Trim();
        }

        public string AttributeValue(IWebElement element, string attribute)
        {
            return element.GetAttribute(attribute)?.Trim();
        }

        public bool HasAttribute(IWebElement element, string attribute)
        {
            return element.GetAttribute(attribute) is not null;
        }

        public bool HasNotAttribute(IWebElement element, string attribute)
        {
            return !HasAttribute(element, attribute);
        }

        public IWebElement ReturnElementIfPresent(
            string element,
            string type,
            string parameterizedCategory = null,
            string option = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info($"Checking if element --{element}-- of type --{type}-- is present or not");

            By locator = GetGenericLocatorFromJson(
                element,
                type,
                option,
                parameterizedCategory,
                checkRelative: (relativeTo is not null)
            );
            
            IWebElement elem = null;

            try
            {
                elem = FindSingleWithRetries(
                    locator,
                    relativeTo: relativeTo,
                    waiter: ElementIsVisible(),
                    retries: retries,
                    sleep: sleep,
                    waitElementTimeout: waitElementTimeout
                );
            }
            catch(Exception e)
            {
                Logger.Info($"Exception raised finding the element: {e.Message}");
            }

            string presentString = (elem is null) ? "No" : "Yes";
            Logger.Info($"Is --{element}-- present in the page?: {presentString}");

            return elem;
        }

        public bool ElementIsPresent(
            string element,
            string type,
            string parameterizedCategory = null,
            string option = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            IWebElement elem = ReturnElementIfPresent(
                element, type, parameterizedCategory, option,
                relativeTo, retries, sleep, waitElementTimeout
            );
            return elem is not null;
        }

        public bool ElementIsNotPresent(
            string element,
            string type,
            string parameterizedCategory = null,
            string option = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            return !ElementIsPresent(
                element, type, parameterizedCategory, option, relativeTo,
                retries, sleep, waitElementTimeout
            );
        }
        
    }
}
