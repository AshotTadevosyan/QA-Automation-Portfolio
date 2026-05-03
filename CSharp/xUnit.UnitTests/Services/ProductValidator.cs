using xUnit.UnitTests.Models;

namespace xUnit.UnitTests.Services;

public record ValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    public static ValidationResult Success() => new(true, []);
    public static ValidationResult Failure(params string[] errors) => new(false, errors);
}

public class ProductValidator
{
    private const int MaxNameLength = 100;
    private const int MaxDescriptionLength = 500;

    public ValidationResult Validate(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(product.Name))
            errors.Add("Product name is required.");
        else if (product.Name.Length > MaxNameLength)
            errors.Add($"Product name must not exceed {MaxNameLength} characters.");

        if (product.Price < 0)
            errors.Add("Price cannot be negative.");

        if (product.StockQuantity < 0)
            errors.Add("Stock quantity cannot be negative.");

        if (!string.IsNullOrEmpty(product.Description) && product.Description.Length > MaxDescriptionLength)
            errors.Add($"Description must not exceed {MaxDescriptionLength} characters.");

        if (string.IsNullOrWhiteSpace(product.Category))
            errors.Add("Category is required.");

        return errors.Count > 0 ? ValidationResult.Failure([.. errors]) : ValidationResult.Success();
    }
}
