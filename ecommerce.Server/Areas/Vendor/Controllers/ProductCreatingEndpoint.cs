using eCommerce.Data;
using Marten;
using Marten.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;
using static ecommerce.Server.Services.ProductRequest;
using static SharedContracts.Events;
using Wolverine.Http.Marten;
using Marten.Pagination;

namespace ecommerce.Server.Areas.Vendor.Controllers
{
    // TODO: Make this just product DTO maybe
    public record ProductCreationResponse(ProductCreated product) : CreationResponse(product.Id.ToString());
    
    [Tags("Vendors")]
    public static class ProductCreatingEndpoint
    {
        [WolverinePost("/api/vendors/products")] // POST /api/vendors/products
        public async static Task<ProductCreationResponse> Create(CreateProduct command, IMessageBus bus)
        {
            ProductCreated product = await bus.InvokeAsync<ProductCreated>(command);

            return new ProductCreationResponse(product);
        }
    }

    [Tags("Vendors")]
    [ProducesResponseType(404)]
    public static class ProductGetEndpoint
    {
        public class ProductQuery
        {
            public int PageSize { get; set; } = 10;
            public int PageNumber { get; set; } = 1;
        }
        [WolverineGet("/api/vendors/products/{id}")]
        public static Product Get([Document] Product product)
        {
            return product;
        }

        [WolverineGet("/api/vendors/products")]
        public async static Task<IPagedList<Product>> GetProducts([FromQuery] ProductQuery query, IQuerySession session, CancellationToken token)
        {
            IQueryable<Product> queryable = session.Query<Product>()
                .OrderBy(x => x.Id);

            return await queryable.ToPagedListAsync(query.PageNumber, query.PageSize, token);
        }
    }
}
