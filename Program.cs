using System.Text.Json;
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
        await context.Response.WriteAsync(JsonSerializer.Serialize(products));
    }
);

// GET /products/{Id}
app.MapGet(
    "/products/{id:int}",
    async (HttpContext context, int id) =>
    {
        var product = products.Where(temp => temp.Id == id).FirstOrDefault();
        if (product == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Incorrect Product Id");
            return;
        }
        await context.Response.WriteAsync(JsonSerializer.Serialize(product));
    }
);

// POST /products
app.MapPost(
    "/products",
    async (HttpContext context, Product product) =>
    {
        products.Add(product);
        await context.Response.WriteAsync("Product added");
    }
);

app.Run();
