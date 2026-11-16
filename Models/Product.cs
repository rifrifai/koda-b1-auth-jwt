
namespace KodaStore.Api.Models;

public sealed class Product
{
    // [Required]
    // [StringLength(30)]
    public string ProductCode { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string Category { get; private set; }
    public string? Description { get; private set; }
    public int? CategoryId { get; private set; }

    // BARU: navigation property; namanya dibedakan dari Category string lama.
    public Category? CategoryEntity { get; private set; }

    // BARU: token konkurensi SQL Server untuk mendeteksi lost update.
    public byte[] RowVersion { get; private set; } = [];
    public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

    public Product(string productCode, string name, decimal price, int stock, string category, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(productCode) || productCode.Trim().Length > 30)
        {
            throw new ArgumentException("Product code is Required, Max 30 character", nameof(productCode));
        }
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 120)
        {
            throw new ArgumentException("Product code is Required", nameof(name));
        }
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative");
        }
        if (stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stock), "stock cannot be negative");
        }
        if (string.IsNullOrWhiteSpace(category) || category.Trim().Length > 30)
        {
            throw new ArgumentException("Category code is Required", nameof(category));
        }
        ProductCode = productCode.Trim().ToUpperInvariant();
        Name = name.Trim();
        Price = price;
        Stock = stock;
        Category = category.Trim();
        Description = description;
    }

    public void UpdateDetail(string name, decimal price, int stock, string category, string? description)
    {
        var validated = new Product(ProductCode, name, price, stock, category, description);

        Name = validated.Name;
        Price = validated.Price;
        Stock = validated.Stock;
        Category = validated.Category;
        Description = validated?.Description;
    }

    public void AssignCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        if (category.CategoryId <= 0)
            throw new InvalidOperationException("Simpan Category terlebih dahulu agar dapat CategoryId");

        CategoryId = category.CategoryId;
        CategoryEntity = category;
        Category = category.Name;
    }
}