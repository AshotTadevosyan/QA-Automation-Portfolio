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
        var addButtons = Driver.FindElements(
            By.CssSelector(".inventory_item button[id^='add-to-cart']"));
        if (index >= addButtons.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        addButtons[index].Click();
    }

    public void RemoveProductFromCartByIndex(int index = 0)
    {
        var removeButtons = Driver.FindElements(
            By.CssSelector(".inventory_item button[id^='remove']"));
        if (index >= removeButtons.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        removeButtons[index].Click();
    }

    public string GetProductNameByIndex(int index = 0) =>
        Driver.FindElements(By.ClassName("inventory_item_name"))[index].Text;

    public CartPage GoToCart()
    {
        WaitForElement(CartLink).Click();
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
