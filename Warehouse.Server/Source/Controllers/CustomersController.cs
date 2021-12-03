using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IEnumerable<CustomerModel>> Get()
        {
            return (await _mediator.Send(new GetCustomers.Request(), CancellationToken.None)).Select(c => new CustomerModel(c));
        }
    }
}
