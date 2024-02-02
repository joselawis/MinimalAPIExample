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
                (HttpContext context, Product product) =>
                {
                    products.Add(product);
                    return Results.Ok(new { message = "Product Added" });
                }
            );

            // PUT /products/{id}
            group.MapPut(
                "/{id:int}",
                (HttpContext context, int id, [FromBody] Product product) =>
                {
                    var productFromCollection = products
                        .Where(temp => temp.Id == id)
                        .FirstOrDefault();
                    if (productFromCollection == null)
                    {
                        return Results.BadRequest(new { error = "Incorrect Product Id" });
                    }
                    productFromCollection.ProductName = product.ProductName;
                    return Results.Ok(new { message = "Product Updated" });
                }
            );

            // DELETE /products/{id}
            group.MapDelete(
                "/{id:int}",
                (HttpContext context, int id) =>
                {
                    var productFromCollection = products
                        .Where(temp => temp.Id == id)
                        .FirstOrDefault();
                    if (productFromCollection == null)
                    {
                        return Results.BadRequest(new { error = "Incorrect Product Id" });
                    }
                    products.Remove(productFromCollection);
                    return Results.Ok(new { message = "Product Deleted" });
                }
            );

            return group;
        }
    }
}
