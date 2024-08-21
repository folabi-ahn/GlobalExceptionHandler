using Carter;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GlobalExceptionHandler.Api;

public static class GetProduct
{
    public record Query(int Id) : IRequest<ProductResponse>;

    public class Handler(ApplicationDbContext dbContext) : IRequestHandler<Query, ProductResponse>
    {
        public async Task<ProductResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if(product is null)
            {
                throw new NotFoundException(request.Id);
            }

            return product.Adapt<ProductResponse>();
        }
    }
}

public class GetProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products/{id}", async (int id, ISender sender) => 
        {
            var query = new GetProduct.Query(id);
            var product = await sender.Send(query);

            return Results.Ok(product);
        });
    }
}
