using FluentAssertions;
using Selenium.WebTests.Config;
using Xunit;

namespace Selenium.WebTests.Tests;

[Trait("Category", "Cart")]
public class CartTests : BaseTest
{
    public CartTests()
    {
        LoginPage.LoginAs(TestSettings.ValidUsername, TestSettings.ValidPassword);
    }

    [Fact]
    public void AddItem_ToCart_ShouldIncrementBadgeCount()
    {
        var inventory = new Pages.InventoryPage(Driver);
        inventory.AddProductToCartByIndex(0);

        inventory.CartItemCount.Should().Be(1);
    }

    [Fact]
    public void AddMultipleItems_ShouldReflectCorrectBadgeCount()
    {
        var inventory = new Pages.InventoryPage(Driver);
        inventory.AddProductToCartByIndex(0);
        inventory.AddProductToCartByIndex(1);

        inventory.CartItemCount.Should().Be(2);
    }

    [Fact]
    public void RemoveItem_FromCart_ShouldDecrementBadgeCount()
    {
        var inventory = new Pages.InventoryPage(Driver);
        inventory.AddProductToCartByIndex(0);
        inventory.AddProductToCartByIndex(1);
        inventory.RemoveProductFromCartByIndex(0);

        inventory.CartItemCount.Should().Be(1);
    }

    [Fact]
    public void GoToCart_ShouldShowAddedItems()
    {
        var inventory = new Pages.InventoryPage(Driver);
        var productName = inventory.GetProductNameByIndex(0);
        inventory.AddProductToCartByIndex(0);

        var cart = inventory.GoToCart();

        cart.IsLoaded.Should().BeTrue();
        cart.ItemCount.Should().Be(1);
        cart.ItemNames_.Should().Contain(productName);
    }

    [Fact]
    public void CompleteCheckout_ShouldShowOrderConfirmation()
    {
        var inventory = new Pages.InventoryPage(Driver);
        inventory.AddProductToCartByIndex(0);

        var checkout = inventory.GoToCart()
                                .ProceedToCheckout()
                                .FillShippingInfo("Jane", "Doe", "12345")
                                .FinishOrder();

        checkout.IsOrderConfirmed.Should().BeTrue();
    }
}
