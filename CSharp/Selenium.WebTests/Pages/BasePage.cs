using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.WebTests.Config;

namespace Selenium.WebTests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestSettings.DefaultTimeoutSeconds));
    }

    protected IWebElement WaitForElement(By locator) =>
        Wait.Until(d => d.FindElement(locator));

    protected bool IsElementVisible(By locator)
    {
        try { return Driver.FindElement(locator).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public string PageTitle => Driver.Title;
    public string CurrentUrl => Driver.Url;
}
