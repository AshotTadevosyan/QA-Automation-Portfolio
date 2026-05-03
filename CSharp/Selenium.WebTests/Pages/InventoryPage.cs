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

        // product slug: "add-to-cart-sauce-labs-backpack" → "sauce-labs-backpack"
        string slug = buttons[index].GetAttribute("id").Replace("add-to-cart-", "");
        ((OpenQA.Selenium.IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", buttons[index]);

        // Wait for the matching Remove button to appear (confirms the click fired).
        Wait.Until(d => d.FindElements(By.Id($"remove-{slug}")).Count > 0);
    }

    public void RemoveProductFromCartByIndex(int index = 0)
    {
        var buttons = Wait.Until(d =>
        {
            var b = d.FindElements(By.CssSelector(".inventory_item button[id^='remove']"));
            return b.Count > index ? b : null;
        })!;

        string slug = buttons[index].GetAttribute("id").Replace("remove-", "");
        ((OpenQA.Selenium.IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", buttons[index]);

        // Wait for the Add-to-cart button to re-appear, confirming removal fired.
        Wait.Until(d => d.FindElements(By.Id($"add-to-cart-{slug}")).Count > 0);
    }

    public string GetProductNameByIndex(int index = 0) =>
        Driver.FindElements(By.ClassName("inventory_item_name"))[index].Text;

    public CartPage GoToCart()
    {
        WaitForElement(CartLink).Click();
        Wait.Until(d => d.Url.Contains("cart"));
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
        WaitForElement(BurgerMenu).Click();
        // SauceDemo's sidebar slides in via CSS animation; WaitForClickable isn't
        // enough — the element is "clickable" before the transition fully settles.
        // A JS click bypasses the animation overlay reliably.
        var logoutLink = WaitForClickable(LogoutLink);
        ((OpenQA.Selenium.IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", logoutLink);
        Wait.Until(d => d.Url == "https://www.saucedemo.com/");
        return new LoginPage(Driver);
    }
}
