using FluentAssertions;
using xUnit.UnitTests.Models;
using xUnit.UnitTests.Services;

namespace xUnit.UnitTests.Tests;

public class ShoppingCartTests
{
    private readonly ShoppingCartService _cart = new();

    private static Product MakeProduct(int id = 1, decimal price = 10.00m) =>
        new() { Id = id, Name = $"Product {id}", Price = price, StockQuantity = 50, Category = "Electronics" };

    // --- AddItem ---

    [Fact]
    public void AddItem_NewProduct_ShouldAppearInCart()
    {
        _cart.AddItem(MakeProduct());

        _cart.Items.Should().HaveCount(1);
    }

    [Fact]
    public void AddItem_ShouldIncrementCartCount()
    {
        _cart.AddItem(MakeProduct(1), 3);

        _cart.Count.Should().Be(3);
    }

    [Fact]
    public void AddItem_SameProductTwice_ShouldMergeIntoOneLineItem()
    {
        var product = MakeProduct();
        _cart.AddItem(product, 2);
        _cart.AddItem(product, 3);

        _cart.Items.Should().HaveCount(1);
        _cart.Count.Should().Be(5);
    }

    [Fact]
    public void AddItem_ShouldCalculateTotalCorrectly()
    {
        _cart.AddItem(MakeProduct(1, 15.00m), 2);
        _cart.AddItem(MakeProduct(2, 5.50m), 4);

        _cart.Total.Should().Be(52.00m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddItem_InvalidQuantity_ShouldThrowArgumentException(int qty)
    {
        var act = () => _cart.AddItem(MakeProduct(), qty);

        act.Should().Throw<ArgumentException>().WithParameterName("quantity");
    }

    [Fact]
    public void AddItem_NullProduct_ShouldThrowArgumentNullException()
    {
        var act = () => _cart.AddItem(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddItem_ExceedsCapacity_ShouldThrowInvalidOperationException()
    {
        _cart.AddItem(MakeProduct(1), 10);

        var act = () => _cart.AddItem(MakeProduct(2), 1);

        act.Should().Throw<InvalidOperationException>().WithMessage("*10*");
    }

    // --- RemoveItem ---

    [Fact]
    public void RemoveItem_ExistingProduct_ShouldBeRemovedFromCart()
    {
        _cart.AddItem(MakeProduct(1));
        _cart.AddItem(MakeProduct(2));

        _cart.RemoveItem(1);

        _cart.Items.Should().HaveCount(1);
        _cart.Items[0].Product.Id.Should().Be(2);
    }

    [Fact]
    public void RemoveItem_NonExistentProduct_ShouldThrowKeyNotFoundException()
    {
        var act = () => _cart.RemoveItem(99);

        act.Should().Throw<KeyNotFoundException>();
    }

    // --- UpdateQuantity ---

    [Fact]
    public void UpdateQuantity_ShouldReflectNewCount()
    {
        _cart.AddItem(MakeProduct(1), 2);

        _cart.UpdateQuantity(1, 7);

        _cart.Count.Should().Be(7);
    }

    // --- ApplyDiscount ---

    [Theory]
    [InlineData(0,   100.00)]
    [InlineData(10,   90.00)]
    [InlineData(50,   50.00)]
    [InlineData(100,   0.00)]
    public void ApplyDiscount_ShouldReturnCorrectDiscountedTotal(decimal pct, decimal expected)
    {
        _cart.AddItem(MakeProduct(1, 100.00m));

        _cart.ApplyDiscount(pct).Should().Be(expected);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void ApplyDiscount_InvalidPercentage_ShouldThrowArgumentOutOfRangeException(decimal pct)
    {
        _cart.AddItem(MakeProduct());

        var act = () => _cart.ApplyDiscount(pct);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    // --- Empty cart ---

    [Fact]
    public void Total_EmptyCart_ShouldBeZero()
    {
        _cart.Total.Should().Be(0m);
    }

    [Fact]
    public void Clear_ShouldEmptyTheCart()
    {
        _cart.AddItem(MakeProduct(1));
        _cart.AddItem(MakeProduct(2));

        _cart.Clear();

        _cart.Items.Should().BeEmpty();
        _cart.Total.Should().Be(0m);
    }
}
