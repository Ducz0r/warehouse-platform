using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Data.Domain;
using Warehouse.Server.Data.Handlers.Utils;

namespace Warehouse.Server.Data.Handlers
{
    public class FindCustomerByName
    {
        public record Request(string Name) : IRequest<DataHandlerResponse<Customer>>;
        public class Handler : IRequestHandler<Request, DataHandlerResponse<Customer>>
        {
            private readonly IDataContext _dataContext;

            public Handler(IDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<DataHandlerResponse<Customer>> Handle(Request request, CancellationToken cancellationToken)
            {
                var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);
                if (customer == null)
                {
                    return DataHandlerResponseFactory<Customer>.ErrorResponse(ResponseError.ElementNotFound);
                } else
                {
                    return DataHandlerResponseFactory<Customer>.SuccessResponse(customer);
                }
            }
        }
    }
}
