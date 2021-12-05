using MediatR;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.ClientApp.Common;
using Warehouse.ClientApp.Handlers.Web;
using Warehouse.ClientApp.Handlers.Web.Utils;
using Warehouse.ClientApp.Models;

namespace Warehouse.ClientApp.Handlers
{
    public class Login
    {
        public record Request(string Name, SecureString Password) : IRequest<WebRequestResult>;
        public class Handler : IRequestHandler<Request, WebRequestResult>
        {
            private readonly IMediator _mediator;
            private readonly ICurrentCredentials _currentCredentials;

            public Handler(IMediator mediator, ICurrentCredentials currentCredentials)
            {
                _mediator = mediator;
                _currentCredentials = currentCredentials;
            }

            public async Task<WebRequestResult> Handle(Request request, CancellationToken cancellationToken)
            {
                // First, attempt to authenticate via web server
                var result = await _mediator.Send(new Authenticate.Request(request.Name, request.Password), cancellationToken);

                // Only set the "currently logged" credentials if authentication was successful
                if (result.Status == WebRequestResultStatus.Success)
                {
                    // Retrieve customer object from the server
                    var customer = (ICustomerModel)result.Content;

                    // Login the current credentials
                    _currentCredentials.Login(customer.Id, request.Name, request.Password);
                }

                return result;
            }
        }
    }
}
