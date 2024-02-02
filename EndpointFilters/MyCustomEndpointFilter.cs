using System.ComponentModel.DataAnnotations;
using MinimalAPI.Models;

namespace MinimalAPI.EndpointFilters
{
    public class MyCustomEndpointFilter : IEndpointFilter
    {
        private readonly ILogger<MyCustomEndpointFilter> _logger;

        public MyCustomEndpointFilter(ILogger<MyCustomEndpointFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
        )
        {
            // Before logic
            _logger.LogInformation("Endpoint filter - before logic");
            var product = context.Arguments.OfType<Product>().FirstOrDefault();
            if (product == null)
            {
                return Results.BadRequest("Product details are not found in the request");
            }
            var validationContext = new ValidationContext(product);
            var errors = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, validationContext, errors, true);
            if (!isValid)
            {
                return Results.BadRequest(new { errors = errors.FirstOrDefault()?.ErrorMessage });
            }

            var result = await next(context); // Calls subsequent filter or endpoint

            // After logic
            _logger.LogInformation("Endpoint filter - after logic");

            return result;
        }
    }
}
