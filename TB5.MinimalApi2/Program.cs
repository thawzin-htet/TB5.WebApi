using TB5.WebApi.Database;
using Microsoft.EntityFrameworkCore;
using TB5.WebApi.Database.AppDbContextModels;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https:
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register ApplicationDbContext for EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Service Implementations (Pick one or use Keyed Services if needed)
// For this example, we register all three but choose EF as the default for IProductService
builder.Services.AddScoped<ProductEfService>();
builder.Services.AddScoped<ProductDapperService>();
builder.Services.AddScoped<ProductAdoService>();

// Dependency Injection: Register the desired implementation for IProductService
builder.Services.AddScoped<IProductService, ProductEfService>(); // Switch to ProductDapperService or ProductAdoService here

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.MapGet("/api/product", async (AppDbContext db) =>
{
    var products = await db.TblProducts
        .AsNoTracking()
        .Where(x => !x.IsDelete)
        .ToListAsync();

    return Results.Ok(products);
})
.WithName("GetProducts")
.WithOpenApi();

app.Run();


