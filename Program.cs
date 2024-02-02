using System.Text.Json;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new List<Product>()
{
    new() { Id = 1, ProductName = "Smartphone" },
    new() { Id = 2, ProductName = "SmartTv" }
};

var productsMapGroup = app.MapGroup("/products");

// GET /products
productsMapGroup.MapGet(
    "/",
    async (HttpContext context) =>
    {
        await context.Response.WriteAsync(JsonSerializer.Serialize(products));
    }
);

// GET /products/{Id}
productsMapGroup.MapGet(
    "/{id:int}",
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
productsMapGroup.MapPost(
    "/",
    async (HttpContext context, Product product) =>
    {
        products.Add(product);
        await context.Response.WriteAsync("Product added");
    }
);

app.Run();
