using OpenQA.Selenium;
using Selenium.WebTests.Helpers;
using Selenium.WebTests.Pages;

namespace Selenium.WebTests.Tests;

public abstract class BaseTest : IDisposable
{
    protected readonly IWebDriver Driver;
    protected readonly LoginPage LoginPage;

    protected BaseTest()
    {
        Driver = DriverFactory.CreateChromeDriver(headless: true);
        LoginPage = new LoginPage(Driver);
        LoginPage.Navigate();
    }

    public void Dispose()
    {
        Driver.Quit();
        Driver.Dispose();
    }
}
