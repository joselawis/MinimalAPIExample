using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Models;

namespace MinimalAPI.RouteGroups
{
    public static class ProductsMapGroup
    {
        static readonly List<Product> products =
            new()
            {
                new() { Id = 1, ProductName = "Smartphone" },
                new() { Id = 2, ProductName = "SmartTv" }
            };

        public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
        {
            // GET /products
            group.MapGet(
                "/",
                async (HttpContext context) =>
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(products));
                }
            );

            // GET /products/{Id}
            group.MapGet(
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
            group.MapPost(
                "/",
                async (HttpContext context, Product product) =>
                {
                    products.Add(product);
                    await context.Response.WriteAsync("Product added");
                }
            );

            // PUT /products/{id}
            group.MapPut(
                "/{id:int}",
                async (HttpContext context, int id, [FromBody] Product product) =>
                {
                    var productFromCollection = products
                        .Where(temp => temp.Id == id)
                        .FirstOrDefault();
                    if (productFromCollection == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Incorrect Product Id");
                        return;
                    }
                    productFromCollection.ProductName = product.ProductName;
                    await context.Response.WriteAsync("Product Updated");
                }
            );

            return group;
        }
    }
}
