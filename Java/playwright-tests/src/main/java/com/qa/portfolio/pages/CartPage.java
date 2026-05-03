package com.qa.portfolio.pages;

import com.microsoft.playwright.Page;

import java.util.List;

public class CartPage extends BasePage {

    private static final String CART_ITEMS      = ".cart_item";
    private static final String ITEM_NAMES      = ".inventory_item_name";
    private static final String CHECKOUT_BTN    = "#checkout";
    private static final String CONTINUE_BTN    = "#continue-shopping";

    public CartPage(Page page) {
        super(page);
    }

    public boolean isLoaded() {
        return getCurrentUrl().contains("cart");
    }

    public int getItemCount() {
        return locate(CART_ITEMS).count();
    }

    public List<String> getItemNames() {
        return locate(ITEM_NAMES).allTextContents();
    }

    public CheckoutPage proceedToCheckout() {
        locate(CHECKOUT_BTN).click();
        return new CheckoutPage(page);
    }

    public InventoryPage continueShopping() {
        locate(CONTINUE_BTN).click();
        return new InventoryPage(page);
    }
}
