using Microsoft.Data.SqlClient;
using System.Data;
using TB5.MinimalApi2.Models;

namespace TB5.MinimalApi2.Services;

public class ProductAdoService : IProductService
{
    private readonly string _connectionString;

    public ProductAdoService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = new List<Product>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SELECT Id, Name, Price, StockQuantity FROM Products", connection);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                StockQuantity = reader.GetInt32(3)
            });
        }
        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("SELECT Id, Name, Price, StockQuantity FROM Products WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                StockQuantity = reader.GetInt32(3)
            };
        }
        return null;
    }

    public async Task<int> CreateAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("INSERT INTO Products (Name, Price, StockQuantity) VALUES (@Name, @Price, @StockQuantity); SELECT SCOPE_IDENTITY()", connection);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task UpdateAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("UPDATE Products SET Name = @Name, Price = @Price, StockQuantity = @StockQuantity WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", product.Id);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
