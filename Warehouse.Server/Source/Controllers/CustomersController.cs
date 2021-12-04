using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Data.Handlers;
using Warehouse.Server.Models;

namespace Warehouse.Server.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IMediator _mediator;

        public CustomersController(ILogger<CustomersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<CustomerModel>))]
        public async Task<IActionResult> Get()
        {
            var dataResponse = await _mediator.Send(new GetCustomers.Request(), CancellationToken.None);

            return Ok(dataResponse.Object.Select(c => new CustomerModel(c)));
        }

        [HttpPost("{id}/increase-quantity")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<CustomerModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> IncreaseQuantity(Guid id, [FromBody]JsonElement json)
        {
            var quantity = json.GetProperty("increase").GetInt32();
            
            var dataResponse = await _mediator.Send(new IncreaseCustomerQuantity.Request(id, quantity), CancellationToken.None);
            
            if (dataResponse.IsSuccess)
            {
                return Ok(new CustomerModel(dataResponse.Object));
            } else
            {
                return NotFound();
            }
        }
    }
}
