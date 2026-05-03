package com.qa.portfolio.tests;

import com.qa.portfolio.base.BaseTest;
import com.qa.portfolio.pages.InventoryPage;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class InventoryTests extends BaseTest {

    private static final String VALID_USER = "standard_user";
    private static final String VALID_PASS = "secret_sauce";

    private InventoryPage inventoryPage;

    @BeforeMethod(alwaysRun = true)
    public void login() {
        inventoryPage = loginPage.loginAs(VALID_USER, VALID_PASS);
    }

    @Test(description = "Inventory page should display 6 products")
    public void inventoryPage_shouldDisplaySixProducts() {
        assertThat(inventoryPage.getProductCount()).isEqualTo(6);
    }

    @Test(description = "Adding one item should increment cart badge to 1")
    public void addOneItem_shouldShowBadgeCountOfOne() {
        inventoryPage.addToCartByIndex(0);

        assertThat(inventoryPage.getCartBadgeCount()).isEqualTo(1);
    }

    @Test(description = "Adding two items should increment cart badge to 2")
    public void addTwoItems_shouldShowBadgeCountOfTwo() {
        inventoryPage.addToCartByIndex(0);
        inventoryPage.addToCartByIndex(1);

        assertThat(inventoryPage.getCartBadgeCount()).isEqualTo(2);
    }

    @Test(description = "Removing an added item should decrement cart badge")
    public void removeItem_shouldDecrementCartBadge() {
        inventoryPage.addToCartByIndex(0);
        inventoryPage.addToCartByIndex(1);
        inventoryPage.removeFromCartByIndex(0);

        assertThat(inventoryPage.getCartBadgeCount()).isEqualTo(1);
    }

    @Test(description = "Sorting by name Z-A should reorder products")
    public void sortByNameDescending_shouldReorderProducts() {
        var before = inventoryPage.getAllProductNames();
        inventoryPage.selectSortOption("za");
        var after = inventoryPage.getAllProductNames();

        assertThat(after).isNotEqualTo(before);
        assertThat(after.get(0)).isGreaterThan(after.get(after.size() - 1));
    }

    @Test(description = "Navigating to cart should show the correct item name")
    public void goToCart_shouldShowAddedItemName() {
        var name = inventoryPage.getProductNameByIndex(0);
        inventoryPage.addToCartByIndex(0);
        var cart = inventoryPage.goToCart();

        assertThat(cart.isLoaded()).isTrue();
        assertThat(cart.getItemNames()).contains(name);
    }
}
