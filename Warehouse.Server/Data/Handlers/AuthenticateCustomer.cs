using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Auth;
using Warehouse.Server.Data.Domain;
using Warehouse.Server.Data.Handlers.Utils;

namespace Warehouse.Server.Data.Handlers
{
    public class AuthenticateCustomer
    {
        public record Request(string CustomerName, string Password) : IRequest<DataHandlerResponse<Customer>>;
        public class Handler : IRequestHandler<Request, DataHandlerResponse<Customer>>
        {
            private readonly IDataContext _dataContext;

            public Handler(IDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<DataHandlerResponse<Customer>> Handle(Request request, CancellationToken cancellationToken)
            {
                var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Name == request.CustomerName, cancellationToken);
                if (customer == null)
                {
                    return DataHandlerResponseFactory<Customer>.ErrorResponse(ResponseError.ElementNotFound);
                }

                if (customer.VerifyPassword(request.Password))
                {
                    return DataHandlerResponseFactory<Customer>.SuccessResponse(customer);
                } else
                {
                    return DataHandlerResponseFactory<Customer>.ErrorResponse(ResponseError.PasswordDoesNotMatch);
                }
            }
        }
    }
}
