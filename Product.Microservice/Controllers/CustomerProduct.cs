using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Product.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProduct : ControllerBase
    {
        private readonly IBus _busService;

        public CustomerProduct(IBus busService)
        {
            _busService = busService;
        }

        [HttpPost]
        public async Task<string> CreateProduct(Shared.Models.Models.CustomerProduct product)
        {
            if (product != null)
            {
                product.AddedOnDate = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/productQueue");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(product);
                return "true";
            }
            return "false";
        }

    }
}
