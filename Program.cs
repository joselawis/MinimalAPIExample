using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new List<Product>()
{
    new() { Id = 1, ProductName = "Smartphone" },
    new() { Id = 2, ProductName = "SmartTv" }
};

// GET /products
app.MapGet(
    "/products",
    async (HttpContext context) =>
    {
        var content = string.Join("\n", products.Select(temp => temp.ToString()));
        await context.Response.WriteAsync(content);
    }
);

app.MapPost(
    "/products",
    async (HttpContext context, Product product) =>
    {
        products.Add(product);
        await context.Response.WriteAsync("Product added");
    }
);

app.Run();
