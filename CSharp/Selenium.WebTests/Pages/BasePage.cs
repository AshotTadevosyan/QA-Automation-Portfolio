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

    // JS click is required in CI headless Chrome: some elements swallow Selenium's
    // synthetic click events (animations, React event delegation, overlays).
    protected void JsClick(By locator) =>
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].click();", WaitForClickable(locator));

    protected bool IsElementVisible(By locator)
    {
        try { return Driver.FindElement(locator).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public string PageTitle => Driver.Title;
    public string CurrentUrl => Driver.Url;
}
