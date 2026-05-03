package com.qa.portfolio.tests;

import com.qa.portfolio.base.BaseTest;
import com.qa.portfolio.pages.InventoryPage;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class CheckoutTests extends BaseTest {

    private static final String VALID_USER = "standard_user";
    private static final String VALID_PASS = "secret_sauce";

    private InventoryPage inventoryPage;

    @BeforeMethod(alwaysRun = true)
    public void login() {
        inventoryPage = loginPage.loginAs(VALID_USER, VALID_PASS);
    }

    @Test(description = "Full checkout flow should display order confirmation")
    public void fullCheckout_shouldShowOrderConfirmation() {
        inventoryPage.addToCartByIndex(0);

        var confirmed = inventoryPage
                .goToCart()
                .proceedToCheckout()
                .fillShippingInfo("John", "Doe", "10001")
                .finishOrder()
                .isOrderConfirmed();

        assertThat(confirmed).isTrue();
    }

    @Test(description = "Checkout should show correct item in cart before confirming")
    public void cartBeforeCheckout_shouldContainAddedItem() {
        var productName = inventoryPage.getProductNameByIndex(0);
        inventoryPage.addToCartByIndex(0);

        var cart = inventoryPage.goToCart();

        assertThat(cart.getItemCount()).isEqualTo(1);
        assertThat(cart.getItemNames()).containsExactly(productName);
    }

    @Test(description = "Continuing shopping from cart should return to inventory")
    public void continueShopping_shouldReturnToInventory() {
        inventoryPage.addToCartByIndex(0);
        var backToInventory = inventoryPage.goToCart().continueShopping();

        assertThat(backToInventory.isLoaded()).isTrue();
    }
}
