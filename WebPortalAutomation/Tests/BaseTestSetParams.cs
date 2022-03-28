using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.IE;

namespace WebPortalAutomation
{
    public partial class BaseTest
    {
        private IWebDriver SetBrowser()
        {
            bool headless = bool.Parse((string)TestContext.Parameters["HEADLESS"] ?? "false");
            this.headless = headless;

            string chromeBinary = (string)TestContext.Parameters["CHROME_BINARY"] ?? "default";
            string chromiumBinary = (string)TestContext.Parameters["CHROMIUM_BINARY"] ?? "default";

            string browser = (string)TestContext.Parameters["BROWSER"] ?? "DEFAULT_BROWSER";
            string browserSanitized = browser.Trim().ToUpper();
            this.browser = browserSanitized;

            // Check how to set up drivers: https://www.selenium.dev/documentation/en/webdriver/driver_requirements/#:~:text=Through%20WebDriver%2C%20Selenium%20supports%20all,Explorer%2C%20Opera%2C%20and%20Safari.
            switch(browserSanitized)
            {
                case "CHROME":
                    return new ChromeDriver(SetChromeOptions(headless, chromeBinary));
                case "FIREFOX":
                    return new FirefoxDriver(SetFirefoxOptions(headless));
                case "EDGE":
                    return new EdgeDriver(SetEdgeOptions(headless));
                case "CHROMIUM":
                case "BRAVE":
                    return new ChromeDriver(SetChromiumOptions(headless, chromiumBinary));
                case "SAFARI":
                    return new SafariDriver();

                default:
                    return new ChromeDriver(SetChromeOptions(headless, chromeBinary));
            }
        }

        private static string SetEnvironmentToTest()
        {
            string env = (string)TestContext.Parameters["ENV"] ?? "";
            string environmentSanitized = env.Trim().ToLower();

            switch(environmentSanitized)
            {           
                case "e2e":
                    return "https://digital-e2e.hospicesource.net/";
                case "dev":
                    return "https://digital-dev.hospicesource.net/";
                default:
                    return "https://digital-e2e.hospicesource.net/";
            }
        }

        private static bool SetLogToStandarOutput()
        {
            string stdout = (string)TestContext.Parameters["STDOUT"] ?? "false";
            return bool.Parse(stdout.Trim().ToLower());
        }

        // Could't found a way to make the set browser options polymorfic
        // respect to a browser

        private ChromeOptions SetChromeOptions(bool headless, string chromeBinary)
        {
            ChromeOptions options = new ChromeOptions();
            
            if(headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");   
            }

            if(chromeBinary != "default")
            {
                options.BinaryLocation = chromeBinary;
            }

            return options;
        }

        private ChromeOptions SetChromiumOptions(bool headless, string chromiumBinary)
        {
            ChromeOptions options = new ChromeOptions();
            
            if(headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");   
            }

            if(chromiumBinary != "default")
            {
                options.BinaryLocation = chromiumBinary;
            }

            return options;
        }

        private FirefoxOptions SetFirefoxOptions(bool headless)
        {
            FirefoxOptions options = new FirefoxOptions();
            
            if(headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
                options.AddArgument("-width=1920");
                options.AddArgument("-height=1080"); 
            }

            return options;
        }

        private EdgeOptions SetEdgeOptions(bool headless)
        {
            EdgeOptions options = new EdgeOptions();
            
            if(headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");   
            }
            options.UseChromium = true;

            return options;
        }
    }
}
