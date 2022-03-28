Instructions:

- Remember that the main branch of automation code is "automation-main"

- Install Google Chrome
- Install .NET framework 5.0 (https://dotnet.microsoft.com/download/dotnet/5.0)
- Add needed packages to the project, open a terminal, go to WebPortalAutomation's
directory and run:
    
    $ dotnet add package Microsoft.NET.Test.Sdk --version 16.10.0
    $ dotnet add package NUnit --version 3.13.2
    $ dotnet add package NUnit3TestAdapter --version 4.0.0
    $ dotnet add package Selenium.WebDriver -v 4.0.0-beta4
    $ dotnet add package SeleniumExtras.WaitHelpers --version 1.0.2
    $ dotnet add package Json.NET --version 1.0.33

- Download chromedrive file (https://chromedriver.chromium.org/downloads) (Or the driver of the browser used: https://www.selenium.dev/documentation/en/webdriver/driver_requirements/#quick-reference).
- Copy driver file to PATH directory
(/usr/bin or /usr/local/bin in linux (probably the first one) or mac (probably the second one))
(for windows https://www.selenium.dev/documentation/en/webdriver/driver_requirements/#adding-executables-to-your-path)

Source: https://swimburger.net/blog/dotnet/how-to-ui-test-using-selenium-and-net-core-on-windows-ubuntu-and-maybe-macos

- Set this as environment variables with its correct value:

    - HMSDIGITAL_SITE_MANAGER_EMAIL
    - HMSDIGITAL_SITE_MANAGER_PASSWORD

    - HMSDIGITAL_DRIVER_EMAIL
    - HMSDIGITAL_DRIVER_PASSWORD

    - HMSDIGITAL_CUSTOM_SERVICE_REP_EMAIL
    - HMSDIGITAL_CUSTOM_SERVICE_REP_PASSWORD

    - HMSDIGITAL_CUSTOM_SERVICE_SUPV_EMAIL
    - HMSDIGITAL_CUSTOM_SERVICE_SUPV_PASSWORD

    - HMSDIGITAL_HOSPICE_ADMIN_EMAIL
    - HMSDIGITAL_HOSPICE_ADMIN_PASSWORD

    - HMSDIGITAL_HOSPICE_USER_EMAIL
    - HMSDIGITAL_HOSPICE_USER_PASSWORD

- Run the tests (this command builds before run the tests)

    $ dotnet test

- In MAC when the code runs for the first time with a new browser, maybe a windows will appear asking for give permissions, the permissions must be given in order to run the tests.
