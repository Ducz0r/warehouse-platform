using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Data.Domain;
using Warehouse.Server.Data.Handlers.Utils;

namespace Warehouse.Server.Data.Handlers
{
    public class GetCustomers
    {
        public record Request() : IRequest<DataHandlerResponse<IReadOnlyList<Customer>>>;
        public class Handler : IRequestHandler<Request, DataHandlerResponse<IReadOnlyList<Customer>>>
        {
            private readonly IDataContext _dataContext;

            public Handler(IDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<DataHandlerResponse<IReadOnlyList<Customer>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var customers = await _dataContext.Customers.ToListAsync(cancellationToken);
                return DataHandlerResponseFactory<IReadOnlyList<Customer>>.SuccessResponse(customers);
            }
        }
    }
}
