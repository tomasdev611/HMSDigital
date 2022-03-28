using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {

        public IWebElement Click(
            IWebElement element,
            string typeClick = "normal",
            bool scrollDown = true,
            int retries = NumberOfRetries,
            int sleep = SleepThread
        )
        {
            Logger.Info($"Trying to click web element --{element}--");

            Func<Dictionary<string, object>, IWebElement> clicker;
            switch(typeClick.ToLower())
            {
                case("normal"): clicker = this.SingleClick; break;
                case("javascript"): clicker = this.JavascriptClick; break;
                case("scrollandclick"): clicker = this.ScrollAndClick; break;
                default: throw new Exception("invalid typeClick parameter");
            }

            Dictionary<string, object> args = new Dictionary<string, object>(){
                { "element", element }, { "scrollDown", scrollDown },
                { "retries", retries }, { "sleep", sleep }
            };

            IWebElement clickedElem = FindAndOperateWithRetries(clicker, args);
            Logger.Info($"Web element --{element}-- clicked");

            return clickedElem;
        }

        public IWebElement Click(
            string element,
            string parameterizedCategory = null,
            string typeClick = "normal",
            bool scrollDown = true,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info($"Trying to click element --{element}-- defined in jsons");

            By locator = GetSimpleLocatorFromJson(
                element,
                type: "clickables",
                category: parameterizedCategory
            );

            IWebElement clickedElem = GetMethodAndClick(
                typeClick: typeClick,
                locator: locator,
                scrollDown: scrollDown,
                relativeTo: relativeTo,
                retries: retries,
                sleep: sleep,
                waitElementTimeout: waitElementTimeout
            );
            Logger.Info($"Element --{element}-- from jsons clicked");
            
            return clickedElem;
        }

        // Returns clicked element
        public IWebElement SearchAndClick(
            string searcherElement,
            string input,
            string clickElement,

            string parameterizedCategoryInput = null,
            IWebElement relativeToInput = null,

            string parameterizedCategoryClick = null,
            IWebElement relativeToClick = null,
            string typeClick = "normal",
            bool scrollDown = true,
            
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            SendInput(
                element: searcherElement,
                parameterizedCategory: parameterizedCategoryInput,
                input: input,
                random: false,
                clearBefore: true,
                relativeTo: relativeToInput,
                retries: retries,
                sleep: sleep,
                waitElementTimeout: waitElementTimeout
            );

            return Click(clickElement, parameterizedCategoryClick, typeClick,
                         scrollDown, relativeToClick, retries, sleep, waitElementTimeout);
        }

        private IWebElement GetMethodAndClick(
            string typeClick,
            By locator,
            bool scrollDown,
            IWebElement relativeTo,
            int retries,
            int sleep,
            int waitElementTimeout
        )
        {
            Func<Dictionary<string, object>, IWebElement> clicker;
            switch(typeClick.ToLower())
            {
                case("normal"): clicker = this.SingleClick; break;
                case("javascript"): clicker = this.JavascriptClick; break;
                case("scrollandclick"): clicker = this.ScrollAndClick; break;
                default: throw new Exception("invalid typeClick parameter");
            }

            Dictionary<string, object> args = new Dictionary<string, object>(){
                { "locator", locator },
                { "clicker", clicker },
                { "scrollDown", scrollDown ? "true" : "false" },
                { "relativeTo", relativeTo },
                { "retries", retries },
                { "sleep", sleep },
                { "waitElementTimeout", waitElementTimeout }
            };

            return FindAndOperateWithRetries(this.FindAndExecuteClick, args);
        }

        private IWebElement FindAndExecuteClick(Dictionary<string, object> args)
        {
            IWebElement relativeTo = (IWebElement)args["relativeTo"];

            IWebElement element = FindAndWait(
                args,
                finder: FinderDefault((By)args["locator"], relativeTo),
                waiter: SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable
            );

            // This magic lines do the following: first obtains the clicker method from
            // the args dictionary, then create a new dictionary with the parameters to
            // be used by the clicker and finally invoke the clicker with those
            // parameters. The elemen returned by the clicker is the one that is later
            // returned for this method.
            return ((Func<Dictionary<string, object>, IWebElement>)args["clicker"])(
                new Dictionary<string, object>(){
                    { "element", element }, { "scrollDown", args["scrollDown"] }
                }
            );
        }
        
        private IWebElement SingleClick(Dictionary<string, object> args)
        {
            IWebElement element = (IWebElement)args["element"];
            element.Click();
            return element;
        }

        private IWebElement JavascriptClick(Dictionary<string, object> args)
        {
            IWebElement element = (IWebElement)args["element"];
            bool scrollDown = (bool)args["scrollDown"];

            IJavaScriptExecutor jsdriver = (IJavaScriptExecutor)webdriver;
            jsdriver.ExecuteScript(
                @$"arguments[0].scrollIntoView({scrollDown});
                  arguments[0].click();",
                  element
            );
            return element;
        }

        private IWebElement ScrollAndClick(Dictionary<string, object> args)
        {
            IWebElement element = (IWebElement)args["element"];
            bool scrollDown = (bool)args["scrollDown"];

            IJavaScriptExecutor jsdriver = (IJavaScriptExecutor)webdriver;
            jsdriver.ExecuteScript(
                $"arguments[0].scrollIntoView({scrollDown});",
                element
            );

            element.Click();
            return element;
        }
    }
}
