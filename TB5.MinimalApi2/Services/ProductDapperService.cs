using Dapper;
using Microsoft.Data.SqlClient;
using TB5.MinimalApi2.Models;

namespace TB5.MinimalApi2.Services;

public class ProductDapperService : IProductService
{
    private readonly string _connectionString;

    public ProductDapperService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Product>("SELECT * FROM Products");
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Product product)
    {
        using var connection = CreateConnection();
        var sql = "INSERT INTO Products (Name, Price, StockQuantity) VALUES (@Name, @Price, @StockQuantity); SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, product);
    }

    public async Task UpdateAsync(Product product)
    {
        using var connection = CreateConnection();
        var sql = "UPDATE Products SET Name = @Name, Price = @Price, StockQuantity = @StockQuantity WHERE Id = @Id";
        await connection.ExecuteAsync(sql, product);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM Products WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}
