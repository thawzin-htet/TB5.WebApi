namespace TB5.Domain.Features.Product.Models;

public class ProductUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
