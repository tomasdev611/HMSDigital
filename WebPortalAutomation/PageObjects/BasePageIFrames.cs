using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {

        public IWebElement SwitchToIFrame(string IFrameElement)
        {
            Logger.Info($"Switching to IFrame {IFrameElement}");

            IWebElement frame = WaitElementLoad(IFrameElement);
            webdriver.SwitchTo().Frame(frame);

            Logger.Info($"Already switched to IFrame {IFrameElement}");

            return frame;
        }

        public void ReturnToDefaultIFrame()
        {
            Logger.Info("Returning to default IFrame");
            webdriver.SwitchTo().DefaultContent();
            Logger.Info("Returned to default IFrame");
        }
    }
}
