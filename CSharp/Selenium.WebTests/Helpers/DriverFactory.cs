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
        }
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--disable-gpu");

        return new ChromeDriver(options);
    }
}
