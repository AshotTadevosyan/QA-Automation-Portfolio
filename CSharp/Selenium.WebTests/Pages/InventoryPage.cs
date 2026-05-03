using OpenQA.Selenium;

namespace Selenium.WebTests.Pages;

public class InventoryPage : BasePage
{
    private static readonly By InventoryList   = By.ClassName("inventory_list");
    private static readonly By InventoryItems  = By.ClassName("inventory_item");
    private static readonly By CartBadge       = By.ClassName("shopping_cart_badge");
    private static readonly By CartLink        = By.ClassName("shopping_cart_link");
    private static readonly By BurgerMenu      = By.Id("react-burger-menu-btn");
    private static readonly By LogoutLink      = By.Id("logout_sidebar_link");
    private static readonly By SortDropdown    = By.ClassName("product_sort_container");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    public bool IsLoaded => IsElementVisible(InventoryList);

    public int ProductCount =>
        Driver.FindElements(InventoryItems).Count;

    public int CartItemCount
    {
        get
        {
            try { return int.Parse(Driver.FindElement(CartBadge).Text); }
            catch { return 0; }
        }
    }

    public void AddProductToCartByIndex(int index = 0)
    {
        var buttons = Wait.Until(d =>
        {
            var b = d.FindElements(By.CssSelector(".inventory_item button[id^='add-to-cart']"));
            return b.Count > index ? b : null;
        })!;

        string slug = buttons[index].GetAttribute("id").Replace("add-to-cart-", "");
        var addLocator = By.Id($"add-to-cart-{slug}");
        var removeLocator = By.Id($"remove-{slug}");
        ClickUntil(addLocator, d => d.FindElements(removeLocator).Count > 0);
    }

    public void RemoveProductFromCartByIndex(int index = 0)
    {
        var buttons = Wait.Until(d =>
        {
            var b = d.FindElements(By.CssSelector(".inventory_item button[id^='remove']"));
            return b.Count > index ? b : null;
        })!;

        string slug = buttons[index].GetAttribute("id").Replace("remove-", "");
        var removeLocator = By.Id($"remove-{slug}");
        var addLocator = By.Id($"add-to-cart-{slug}");
        ClickUntil(removeLocator, d => d.FindElements(addLocator).Count > 0);
    }

    public string GetProductNameByIndex(int index = 0) =>
        Driver.FindElements(By.ClassName("inventory_item_name"))[index].Text;

    public CartPage GoToCart()
    {
        ClickUntil(CartLink, d => d.Url.Contains("cart"));
        return new CartPage(Driver);
    }

    public void SelectSortOption(string optionText)
    {
        var select = new OpenQA.Selenium.Support.UI.SelectElement(
            WaitForElement(SortDropdown));
        select.SelectByText(optionText);
    }

    public LoginPage Logout()
    {
        WaitForClickable(BurgerMenu).Click();
        // react-burger-menu sets aria-hidden="false" on .bm-menu-wrap when open.
        Wait.Until(d => d.FindElement(By.CssSelector(".bm-menu-wrap"))
                         .GetAttribute("aria-hidden") == "false");
        WaitForClickable(LogoutLink).Click();
        // SauceDemo's React Router logout sometimes doesn't push a new history
        // entry in headless Chrome. Explicit navigation to root ensures a clean
        // state and doubles as a session-clear verification: if the user is still
        // authenticated, SauceDemo would redirect back to inventory.
        Driver.Navigate().GoToUrl(Config.TestSettings.BaseUrl);
        Wait.Until(d => d.FindElement(By.Id("login-button")) != null);
        return new LoginPage(Driver);
    }
}
