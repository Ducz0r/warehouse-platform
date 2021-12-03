using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Data.Domain;

namespace Warehouse.Server.Data.Handlers
{
    public class GetCustomers
    {
        public record Request() : IRequest<IReadOnlyList<Customer>>;
        public class Handler : IRequestHandler<Request, IReadOnlyList<Customer>>
        {
            private readonly IDataContext _dataContext;

            public Handler(IDataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<IReadOnlyList<Customer>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _dataContext.Customers.ToListAsync(cancellationToken);
            }
        }
    }
}
