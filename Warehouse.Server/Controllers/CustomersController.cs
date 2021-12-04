using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
    [Authorize]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomersController(
            ILogger<CustomersController> logger,
            IMediator mediator,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Get a list of all customers.
        /// </summary>
        /// <returns>Array of all customers</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<CustomerModel>))]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            var dataResponse = await _mediator.Send(new GetCustomers.Request(), CancellationToken.None);

            return Ok(dataResponse.Object.Select(c => new CustomerModel(c)));
        }

        /// <summary>
        /// Increase individual customer's warehouse quantity.
        /// </summary>
        /// <param name="id">Customer's GUID</param>
        /// <param name="json">Body</param>
        /// <returns>The updated customer object</returns>
        /// <remarks>
        /// Request body must adhere to the following format:
        /// {
        ///     "increase": {quantity-increase-integer}
        /// }
        /// </remarks>
        [HttpPost("{id}/increase-quantity")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<CustomerModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> IncreaseQuantity(Guid id, [FromBody]JsonElement json)
        {
            int quantity;

            try
            {
                quantity = json.GetProperty("increase").GetInt32();
            } catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException)
            {
                return BadRequest();
            }
            
            var dataResponse = await _mediator.Send(new IncreaseCustomerQuantity.Request(id, quantity), CancellationToken.None);
            
            if (dataResponse.IsSuccess)
            {
                return Ok(new CustomerModel(dataResponse.Object));
            } else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Re-initializes a list of seeded customers.
        /// WARNING: This will reset/delete all the existing data in the database.
        /// </summary>
        /// <returns>Array of seeded customers.</returns>
        [HttpPost("init")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<CustomerModel>))]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Init()
        {
            if (!_webHostEnvironment.IsDevelopment())
            {
                // TODO: This can be hidden in a better fashion e.g. using
                // authorization policies, or custom attributes
                throw new NotImplementedException();
            }

            var dataResponse = await _mediator.Send(new SeedCustomers.Request(), CancellationToken.None);

            return Ok(dataResponse.Object.Select(c => new CustomerModel(c)));
        }
    }
}
