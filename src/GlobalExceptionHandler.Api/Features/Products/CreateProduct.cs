using Carter;
using FluentValidation;
using Mapster;
using MediatR;

namespace GlobalExceptionHandler.Api;

public static class CreateProduct
{
    public record Command(string Name, decimal Price) : IRequest<ProductResponse>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Price).GreaterThan(0);
        }
    }

    public class Handler 
        (
            ApplicationDbContext dbContext,
            IValidator<Command> validator
        ) 
        : IRequestHandler<Command, ProductResponse>
    {
        public async Task<ProductResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return product.Adapt<ProductResponse>();
        }
    }
}

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/products", async (CreateProductRequest request, ISender sender) => 
        {
            var command = request.Adapt<CreateProduct.Command>();
            var product = await sender.Send(command);
            return Results.Ok(product);
        });
    }
}
