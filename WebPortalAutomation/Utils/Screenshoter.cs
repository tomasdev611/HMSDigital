using System;
using System.IO; // File, Path defined here

using OpenQA.Selenium;
using NUnit.Framework;

namespace WebPortalAutomation
{
    public static class Screenshoter
    {
        public static string TakeScreenshot(
            IWebDriver webdriver,
            string prefix = null,
            string suffix = null,
            string screenshotDescription = null
        )
        {
            // Create filename to save the screenshot
            string contextFileName = Utils.ContextToFileName(); 
            string finalPrefix = (prefix is null) ? "" : $"{prefix}_";
            string finalSuffix = (suffix is null) ? "" : $"_{suffix}";

            string directoryPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            
            string fileName = $"SCREENSHOT_{finalPrefix}{contextFileName}{finalSuffix}.png";
            fileName = fileName.Replace("|", "_").Replace(":", "_");

            Logger.Info($"Taking screenshot {fileName}");

            string filePath = Path.Join(directoryPath, "Screenshots", fileName);
            try
            {
                Screenshot screenshot = (webdriver as ITakesScreenshot).GetScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
                TestContext.AddTestAttachment(filePath, screenshotDescription);
            }
            catch(Exception exc)
            {
                Logger.Critical(
                    $"Could not take screenshot from browser, this error has appeared: {exc.Message}\nStack: {exc.StackTrace}"
                );
                return null;
            }

            return filePath;
        }
    }
}
