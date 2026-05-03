using OpenQA.Selenium;

namespace Selenium.WebTests.Pages;

public class CartPage : BasePage
{
    private static readonly By CartItems      = By.ClassName("cart_item");
    private static readonly By CheckoutButton = By.Id("checkout");
    private static readonly By ContinueButton = By.Id("continue-shopping");
    private static readonly By ItemNames      = By.ClassName("inventory_item_name");

    public CartPage(IWebDriver driver) : base(driver) { }

    public bool IsLoaded => CurrentUrl.Contains("cart");

    public int ItemCount => Driver.FindElements(CartItems).Count;

    public IReadOnlyList<string> ItemNames_ =>
        Driver.FindElements(ItemNames).Select(e => e.Text).ToList();

    public CheckoutPage ProceedToCheckout()
    {
        WaitForElement(CheckoutButton).Click();
        return new CheckoutPage(Driver);
    }

    public InventoryPage ContinueShopping()
    {
        WaitForElement(ContinueButton).Click();
        return new InventoryPage(Driver);
    }
}
