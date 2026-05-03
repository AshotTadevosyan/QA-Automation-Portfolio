using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium.WebTests.Helpers;

public static class DriverFactory
{
    public static IWebDriver CreateChromeDriver(bool headless = true)
    {
        var options = new ChromeOptions();
        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--enable-automation");
        }
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--disable-gpu");

        // browser-actions/setup-chrome sets CHROMEWEBDRIVER to the directory
        // containing the matching chromedriver binary. Using it directly avoids
        // Selenium Manager downloading a mismatched version for CI Chromium.
        var driverDir = Environment.GetEnvironmentVariable("CHROMEWEBDRIVER");
        var service = string.IsNullOrEmpty(driverDir)
            ? ChromeDriverService.CreateDefaultService()
            : ChromeDriverService.CreateDefaultService(driverDir);

        service.SuppressInitialDiagnosticInformation = true;

        return new ChromeDriver(service, options);
    }
}
