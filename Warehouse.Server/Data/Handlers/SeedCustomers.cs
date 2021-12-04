using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Auth;
using Warehouse.Server.Data.Domain;
using Warehouse.Server.Data.Handlers.Utils;

namespace Warehouse.Server.Data.Handlers
{
    public class SeedCustomers
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
                _dataContext.Customers.RemoveRange(_dataContext.Customers); // TODO: This will not behave well for large dataset
                _dataContext.SaveChanges();

                List<Customer> seededCustomers = new List<Customer>()
                {
                    new Customer() {Id = Guid.NewGuid(), Name = "Novartis", Quantity = 0},
                    new Customer() {Id = Guid.NewGuid(), Name = "Evergreen", Quantity = 0},
                    new Customer() {Id = Guid.NewGuid(), Name = "Luka Koper", Quantity = 0}
                };
                seededCustomers[0].AssignPassword("novartis-pass");
                seededCustomers[1].AssignPassword("evergreen-pass");
                seededCustomers[2].AssignPassword("koper-pass");
                await _dataContext.Customers.AddRangeAsync(seededCustomers, cancellationToken);
                await _dataContext.SaveChangesAsync();

                return DataHandlerResponseFactory<IReadOnlyList<Customer>>.SuccessResponse(seededCustomers);
            }
        }
    }
}
