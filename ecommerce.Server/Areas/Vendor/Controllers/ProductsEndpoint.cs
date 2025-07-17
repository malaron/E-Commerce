using eCommerce.Data;
using Marten;
using Marten.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;

namespace ecommerce.Server.Areas.Vendor.Controllers
{
    public class ProductsEndpoint
    {

        private readonly HttpContextAccessor _accessor;

        public ProductsEndpoint(HttpContextAccessor accessor) 
        {
            _accessor = accessor;
        }
        /*
        //    [AggregateHandler]
        [WolverinePost("/api/vendors/products/{id}")]
        public void CreateProduct(Guid id, [FromServices] IQuerySession session)
        {
//            return session.Json.WriteById<>
        }

        [WolverineGet("/api/vendors/products/{id}")]
        public Task GetProduct(Guid id, [FromServices] IQuerySession session)
        {
            return session.Json.WriteById<Product>(id, _accessor.HttpContext!);
        } */
        // The updated version of the Order aggregate will be returned as the response body
        // from requesting this endpoint at runtime
        /*    public static (UpdatedAggregate, Events) AddProduct(ConfirmOrder command, Order order)
            {
                return (
                    new UpdatedAggregate(),
                    [new OrderConfirmed()]
                );
            } */
    }
}
