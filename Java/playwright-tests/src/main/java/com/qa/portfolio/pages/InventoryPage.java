package com.qa.portfolio.pages;

import com.microsoft.playwright.Page;

import java.util.List;

public class InventoryPage extends BasePage {

    private static final String INVENTORY_LIST  = ".inventory_list";
    private static final String ITEM_NAMES      = ".inventory_item_name";
    private static final String ADD_TO_CART_BTN = ".inventory_item button[id^='add-to-cart']";
    private static final String REMOVE_BTN      = ".inventory_item button[id^='remove']";
    private static final String CART_BADGE      = ".shopping_cart_badge";
    private static final String CART_LINK       = ".shopping_cart_link";
    private static final String SORT_DROPDOWN   = ".product_sort_container";
    private static final String BURGER_MENU     = "#react-burger-menu-btn";
    private static final String LOGOUT_LINK     = "#logout_sidebar_link";

    public InventoryPage(Page page) {
        super(page);
    }

    public boolean isLoaded() {
        return locate(INVENTORY_LIST).isVisible();
    }

    public int getProductCount() {
        return locate(ITEM_NAMES).count();
    }

    public String getProductNameByIndex(int index) {
        return locate(ITEM_NAMES).nth(index).textContent();
    }

    public List<String> getAllProductNames() {
        return locate(ITEM_NAMES).allTextContents();
    }

    public void addToCartByIndex(int index) {
        locate(ADD_TO_CART_BTN).nth(index).click();
    }

    public void removeFromCartByIndex(int index) {
        locate(REMOVE_BTN).nth(index).click();
    }

    public int getCartBadgeCount() {
        if (!locate(CART_BADGE).isVisible()) return 0;
        return Integer.parseInt(locate(CART_BADGE).textContent().trim());
    }

    public CartPage goToCart() {
        locate(CART_LINK).click();
        return new CartPage(page);
    }

    public void selectSortOption(String value) {
        page.selectOption(SORT_DROPDOWN, value);
    }

    public LoginPage logout() {
        locate(BURGER_MENU).click();
        waitForVisible(LOGOUT_LINK);
        locate(LOGOUT_LINK).click();
        return new LoginPage(page);
    }
}
