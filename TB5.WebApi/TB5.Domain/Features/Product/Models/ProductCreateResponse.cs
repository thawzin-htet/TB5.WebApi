namespace TB5.Domain.Features.Product.Models;

public class ProductCreateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedDateTime { get; set; }
}
