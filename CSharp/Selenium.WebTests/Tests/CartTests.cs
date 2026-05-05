using FluentAssertions;
using Selenium.WebTests.Config;
using Selenium.WebTests.Pages;
using Xunit;

namespace Selenium.WebTests.Tests;

[Trait("Category", "Cart")]
public class CartTests : BaseTest
{
    private readonly InventoryPage _inventory;

    public CartTests()
    {
        // LoginAs already waits for .inventory_list, but capture the page
        // object it returns rather than discarding it and re-creating one.
        _inventory = LoginPage.LoginAs(TestSettings.ValidUsername, TestSettings.ValidPassword);
    }

    [Fact]
    public void AddItem_ToCart_ShouldIncrementBadgeCount()
    {
        _inventory.AddProductToCartByIndex(0);
        _inventory.CartItemCount.Should().Be(1);
    }

    [Fact]
    public void AddMultipleItems_ShouldReflectCorrectBadgeCount()
    {
        _inventory.AddProductToCartByIndex(0);
        _inventory.AddProductToCartByIndex(1);
        _inventory.CartItemCount.Should().Be(2);
    }

    [Fact]
    public void RemoveItem_FromCart_ShouldDecrementBadgeCount()
    {
        _inventory.AddProductToCartByIndex(0);
        _inventory.AddProductToCartByIndex(1);
        _inventory.RemoveProductFromCartByIndex(0);
        _inventory.CartItemCount.Should().Be(1);
    }

    [Fact]
    public void GoToCart_ShouldShowAddedItems()
    {
        var productName = _inventory.GetProductNameByIndex(0);
        _inventory.AddProductToCartByIndex(0);

        var cart = _inventory.GoToCart();

        cart.IsLoaded.Should().BeTrue();
        cart.ItemCount.Should().Be(1);
        cart.ItemNames_.Should().Contain(productName);
    }

    [Fact]
    public void CompleteCheckout_ShouldShowOrderConfirmation()
    {
        _inventory.AddProductToCartByIndex(0);

        var checkout = _inventory.GoToCart()
                                 .ProceedToCheckout()
                                 .FillShippingInfo("Jane", "Doe", "12345")
                                 .FinishOrder();

        checkout.IsOrderConfirmed.Should().BeTrue();
    }
}
