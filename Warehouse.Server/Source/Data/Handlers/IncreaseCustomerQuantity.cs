using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Data.Domain;
using Warehouse.Server.Data.Handlers.Utils;

namespace Warehouse.Server.Data.Handlers
{
    public class IncreaseCustomerQuantity
    {
        public record Request(Guid CustomerId, int Increase) : IRequest<DataHandlerResponse<Customer>>;
        public class Handler : IRequestHandler<Request, DataHandlerResponse<Customer>>
        {
            private readonly IDataContext _dataContext;

            public Handler(IDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<DataHandlerResponse<Customer>> Handle(Request request, CancellationToken cancellationToken)
            {
                var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);
                if (customer == null)
                {
                    return DataHandlerResponseFactory<Customer>.ErrorResponse(ResponseError.ElementNotFound);
                }

                customer.IncreaseQuantity(request.Increase);
                _dataContext.Customers.Update(customer);
                _dataContext.SaveChanges();

                return DataHandlerResponseFactory<Customer>.SuccessResponse(customer);
            }
        }
    }
}
