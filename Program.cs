using MinimalAPI.RouteGroups;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGroup("/products").ProductsAPI();

app.Run();
