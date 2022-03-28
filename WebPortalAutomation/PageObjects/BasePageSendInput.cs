using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Linq;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {
        public IWebElement SendInput(
            IWebElement element,
            string input = null,
            bool random = false,
            bool clearBefore = false,
            int retries = NumberOfRetries,
            int sleep = SleepThread        )
        {
            if(random || input is null) { input = RandomUtils.generateString(); }

            Logger.Info($"Sending input --{input}-- to web element --{element}--");

            IWebElement elementAfter = FindAndOperateWithRetries(
                method: clearBefore ? this.ClearAndSendInput : this.SendInputToElement,
                args: new Dictionary<string, object>(){
                    { "element", element }, { "input", input },
                    { "retries", retries }, { "sleep", sleep }
                }
            );

            Logger.Info($"Finish of sending input --{input}-- to web element --{element}--");
            return elementAfter;
        }

        public IWebElement SendInput(
            string element,
            string parameterizedCategory = null,
            string input = null,
            bool random = false,
            bool clearBefore = false,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            if(random || input is null) { input = RandomUtils.generateString(); }

            Logger.Info($"Sending input --{input}-- to element --{element}--");

            By locator = GetSimpleLocatorFromJson(
                element: element,
                type: "writables",
                category: parameterizedCategory,
                checkRelative: (relativeTo is not null)
            );

            IWebElement elementLocated = FindAndOperateWithRetries(
                method: clearBefore ? this.FindAndClearAndSendInput : this.FindAndSendInput,
                args: new Dictionary<string, object>(){
                    { "locator", locator }, { "input", input }, { "relativeTo", relativeTo },
                    { "retries", retries }, { "sleep", sleep }, { "waitElementTimeout", waitElementTimeout }
                }
            );

            Logger.Info($"Finish of sending input --{input}-- to element --{element}--");
            return elementLocated;
        }

        public IWebElement DeleteInput(
            string element,
            string parameterizedCategory = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info($"Deleting input from element --{element}--");

            By locator = GetSimpleLocatorFromJson(
                element: element,
                type: "writables",
                category: parameterizedCategory,
                checkRelative: (relativeTo is not null)
            );

            IWebElement elementLocated = FindAndOperateWithRetries(
                method: this.FindAndDeleteText,
                args: new Dictionary<string, object>(){
                    { "locator", locator },
                    { "relativeTo", relativeTo },
                    { "retries", retries },
                    { "sleep", sleep },
                    { "waitElementTimeout", waitElementTimeout }
                }
            );

            Logger.Info($"Finish of deleting input from element --{element}--");
            return elementLocated;
        }

        public IWebElement SendInputAndCloseMenu(
            string element,
            string parameterizedCategory = null,
            string input = null,
            bool random = false,
            bool clearBefore = false,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            IWebElement inputBox = SendInput(
                element,
                parameterizedCategory,
                input,
                random,
                clearBefore,
                relativeTo,
                retries,
                sleep,
                waitElementTimeout
            );

            inputBox.SendKeys(Keys.Tab);
            return inputBox;
        }

        private IWebElement FindAndSendInput(Dictionary<string, object> args)
        {
            args["element"] = (object)FindElementToSendText(args);
            return SendInputToElement(args);
        }

        private IWebElement SendInputToElement(Dictionary<string, object> args)
        {
            IWebElement element = (IWebElement)args["element"];
            element.SendKeys((string)args["input"]);
            return element;
        }

        private IWebElement FindAndClearAndSendInput(Dictionary<string, object> args)
        {
           args["element"] = (object)FindElementToSendText(args);
           return ClearAndSendInput(args); 
        }

        private IWebElement ClearAndSendInput(Dictionary<string, object> args)
        {
            IWebElement element = (IWebElement)args["element"];

            int digitsToDelete = AttributeValue(element, "value").Length;
            
            string backspaceDeletes = string.Concat(
                Enumerable.Repeat(Keys.Backspace, digitsToDelete)
            );
            
            element.SendKeys(Keys.End + backspaceDeletes + (string)args["input"]);
            return element;
        }

        private IWebElement FindAndDeleteText(Dictionary<string, object> args)
        {
            args["element"] = (object)FindElementToSendText(args);
            return DeleteText(args);
        }

        private IWebElement DeleteText(Dictionary<string, object> args)
        {
            IWebElement element = (IWebElement)args["element"];
            element.Clear();
            return element;
        }

        private IWebElement FindElementToSendText(Dictionary<string, object> args)
        {
            IWebElement relativeTo = (IWebElement)args["relativeTo"];
            By locator = (By)args["locator"];
            return FindAndWait(args, finder: FinderDefault(locator, relativeTo));
        }
    }
}
