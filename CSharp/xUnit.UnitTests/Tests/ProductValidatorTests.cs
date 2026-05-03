using FluentAssertions;
using xUnit.UnitTests.Models;
using xUnit.UnitTests.Services;

namespace xUnit.UnitTests.Tests;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator = new();

    private static Product ValidProduct() => new()
    {
        Id = 1,
        Name = "Wireless Headphones",
        Description = "Noise-cancelling over-ear headphones.",
        Price = 79.99m,
        StockQuantity = 20,
        Category = "Electronics"
    };

    [Fact]
    public void Validate_ValidProduct_ShouldReturnSuccess()
    {
        var result = _validator.Validate(ValidProduct());

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_NullProduct_ShouldThrowArgumentNullException()
    {
        var act = () => _validator.Validate(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_MissingName_ShouldFailWithNameError(string? name)
    {
        var product = ValidProduct();
        product.Name = name!;

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainMatch("*name*");
    }

    [Fact]
    public void Validate_NameExceedsMaxLength_ShouldFailWithNameError()
    {
        var product = ValidProduct();
        product.Name = new string('A', 101);

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainMatch("*100*");
    }

    [Fact]
    public void Validate_NegativePrice_ShouldFailWithPriceError()
    {
        var product = ValidProduct();
        product.Price = -0.01m;

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainMatch("*rice*");
    }

    [Fact]
    public void Validate_ZeroPrice_ShouldBeValid()
    {
        var product = ValidProduct();
        product.Price = 0m;

        _validator.Validate(product).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_NegativeStock_ShouldFailWithStockError()
    {
        var product = ValidProduct();
        product.StockQuantity = -1;

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainMatch("*tock*");
    }

    [Fact]
    public void Validate_DescriptionExceedsMaxLength_ShouldFail()
    {
        var product = ValidProduct();
        product.Description = new string('X', 501);

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainMatch("*500*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_MissingCategory_ShouldFailWithCategoryError(string category)
    {
        var product = ValidProduct();
        product.Category = category;

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainMatch("*ategory*");
    }

    [Fact]
    public void Validate_MultipleViolations_ShouldReturnAllErrors()
    {
        var product = new Product
        {
            Name = "",
            Price = -5m,
            StockQuantity = -2,
            Category = ""
        };

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(3);
    }
}
