using Microsoft.AspNetCore.Mvc;
using MinimalAPI.EndpointFilters;
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

        static readonly Dictionary<string, string[]> validationDictionary =
            new() { { "id", new string[] { "Incorrect Product Id" } } };

        public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
        {
            // GET /products
            group.MapGet(
                "/",
                (HttpContext context) =>
                {
                    return Results.Json(products);
                }
            );

            // GET /products/{Id}
            group.MapGet(
                "/{id:int}",
                (HttpContext context, int id) =>
                {
                    var product = products.Where(temp => temp.Id == id).FirstOrDefault();
                    if (product == null)
                    {
                        return Results.ValidationProblem(validationDictionary);
                    }
                    return Results.Json(product);
                }
            );

            // POST /products
            group
                .MapPost(
                    "/",
                    (HttpContext context, Product product) =>
                    {
                        products.Add(product);
                        return Results.Ok(new { message = "Product Added" });
                    }
                )
                .AddEndpointFilter<MyCustomEndpointFilter>();

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
                        return Results.ValidationProblem(validationDictionary);
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
                        return Results.ValidationProblem(validationDictionary);
                    }
                    products.Remove(productFromCollection);
                    return Results.Ok(new { message = "Product Deleted" });
                }
            );

            return group;
        }
    }
}
