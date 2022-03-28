using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {

        public IWebElement SelectOption(
            string element,
            string option = null,
            string parameterizedCategory = null,
            string typeClickMenu = "normal",
            string typeClickOption = "normal",
            bool randomOption = false,
            bool scrollDownMenu = true,
            bool scrollDownOption = true,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info($"Selecting option --{option}-- of element --{element}--");

            Dictionary<string, By> locators = GetSelectableLocatorFromJson(
                element,
                option,
                randomOption,
                checkRelative: (relativeTo is not null)
            );

            IWebElement menu = GetMethodAndClick(
                typeClickMenu,
                locators["menu"],
                scrollDownMenu,
                relativeTo,
                retries,
                sleep,
                waitElementTimeout
            );

            // Click in the option
            GetMethodAndClick(
                typeClickOption,
                locators["option"],
                scrollDownOption,
                relativeTo,
                retries,
                sleep,
                waitElementTimeout
            );

            Logger.Info($"Finish of selecting option --{option}-- of element --{element}--");

            return menu;
        }

        public IWebElement SelectOptionAndCloseMenu(
            string element,
            string option,
            string parameterizedCategory = null,
            string typeClickMenu = "normal",
            string typeClickOption = "normal",
            bool randomOption = false,
            bool scrollDownMenu = true,
            bool scrollDownOption = true,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            IWebElement menu = SelectOption(
                element,
                option,
                parameterizedCategory,
                typeClickMenu,
                typeClickOption,
                randomOption,
                scrollDownMenu,
                scrollDownOption,
                relativeTo,
                retries,
                sleep,
                waitElementTimeout
            );

            menu.SendKeys(Keys.Tab);
            return menu;
        }

    }
}
