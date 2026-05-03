using xUnit.UnitTests.Models;

namespace xUnit.UnitTests.Services;

public class ShoppingCartService
{
    private readonly List<CartItem> _items = new();
    private const int MaxCartCapacity = 10;

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int Count => _items.Sum(i => i.Quantity);
    public decimal Total => _items.Sum(i => i.LineTotal);

    public void AddItem(Product product, int quantity = 1)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (Count + quantity > MaxCartCapacity)
            throw new InvalidOperationException($"Cart cannot hold more than {MaxCartCapacity} items.");

        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing is not null)
            existing.Quantity += quantity;
        else
            _items.Add(new CartItem { Product = product, Quantity = quantity });
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId)
            ?? throw new KeyNotFoundException($"Product {productId} is not in the cart.");
        _items.Remove(item);
    }

    public void UpdateQuantity(int productId, int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

        var item = _items.FirstOrDefault(i => i.Product.Id == productId)
            ?? throw new KeyNotFoundException($"Product {productId} is not in the cart.");
        item.Quantity = newQuantity;
    }

    public decimal ApplyDiscount(decimal percentage)
    {
        if (percentage is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Discount must be between 0 and 100.");
        return Total * (1 - percentage / 100);
    }

    public void Clear() => _items.Clear();
}
