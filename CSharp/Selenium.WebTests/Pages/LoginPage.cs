using OpenQA.Selenium;
using Selenium.WebTests.Config;

namespace Selenium.WebTests.Pages;

public class LoginPage : BasePage
{
    private static readonly By UsernameInput = By.Id("user-name");
    private static readonly By PasswordInput = By.Id("password");
    private static readonly By LoginButton   = By.Id("login-button");
    private static readonly By ErrorMessage  = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public void Navigate() => Driver.Navigate().GoToUrl(TestSettings.BaseUrl);

    public InventoryPage LoginAs(string username, string password)
    {
        WaitForElement(UsernameInput).SendKeys(username);
        Driver.FindElement(PasswordInput).SendKeys(password);
        WaitForClickable(LoginButton).Click();
        // Block until the inventory list renders so callers get a fully-loaded page.
        WaitForElement(By.ClassName("inventory_list"));
        return new InventoryPage(Driver);
    }

    public LoginPage LoginExpectingError(string username, string password)
    {
        WaitForElement(UsernameInput).Clear();
        WaitForElement(UsernameInput).SendKeys(username);
        Driver.FindElement(PasswordInput).Clear();
        Driver.FindElement(PasswordInput).SendKeys(password);
        WaitForClickable(LoginButton).Click();
        return this;
    }

    public bool HasError => IsElementVisible(ErrorMessage);
    public string ErrorText => HasError ? Driver.FindElement(ErrorMessage).Text : string.Empty;
}
