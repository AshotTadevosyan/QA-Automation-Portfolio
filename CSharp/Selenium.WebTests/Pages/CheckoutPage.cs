using OpenQA.Selenium;

namespace Selenium.WebTests.Pages;

public class CheckoutPage : BasePage
{
    private static readonly By FirstNameInput  = By.Id("first-name");
    private static readonly By LastNameInput   = By.Id("last-name");
    private static readonly By PostalCodeInput = By.Id("postal-code");
    private static readonly By ContinueButton  = By.Id("continue");
    private static readonly By FinishButton    = By.Id("finish");
    private static readonly By ConfirmHeader   = By.ClassName("complete-header");

    public CheckoutPage(IWebDriver driver) : base(driver) { }

    public CheckoutPage FillShippingInfo(string firstName, string lastName, string postalCode)
    {
        WaitForElement(FirstNameInput).SendKeys(firstName);
        WaitForElement(LastNameInput).SendKeys(lastName);
        WaitForElement(PostalCodeInput).SendKeys(postalCode);
        ClickUntil(ContinueButton, d => d.Url.Contains("checkout-step-two"));
        return this;
    }

    public CheckoutPage FinishOrder()
    {
        ClickUntil(FinishButton, d => d.Url.Contains("checkout-complete"));
        return this;
    }

    public bool IsOrderConfirmed =>
        IsElementVisible(ConfirmHeader) &&
        Driver.FindElement(ConfirmHeader).Text.Contains("Thank you");
}
