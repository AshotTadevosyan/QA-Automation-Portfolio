using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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

    // Waits until the element is present AND clickable (not obscured or animating).
    protected IWebElement WaitForClickable(By locator) =>
        Wait.Until(ExpectedConditions.ElementToBeClickable(locator));

    // Retries click + condition up to maxAttempts times.
    // Necessary for CI headless Chrome where button events occasionally don't
    // propagate on the first click (network/rendering variance against SauceDemo).
    protected void ClickUntil(By locator, Func<IWebDriver, bool> condition, int maxAttempts = 3)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            WaitForClickable(locator).Click();
            try
            {
                Wait.Until(condition);
                return;
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException) when (attempt < maxAttempts)
            {
                // Click didn't register — retry with a fresh element reference.
            }
        }
    }

    protected bool IsElementVisible(By locator)
    {
        try { return Driver.FindElement(locator).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public string PageTitle => Driver.Title;
    public string CurrentUrl => Driver.Url;
}
