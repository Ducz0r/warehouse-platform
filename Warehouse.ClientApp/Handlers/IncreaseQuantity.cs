using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.ClientApp.Common;
using Warehouse.ClientApp.Handlers.Web.Utils;
using Warehouse.ClientApp.Models;

namespace Warehouse.ClientApp.Handlers
{
    public class IncreaseQuantity
    {
        public record Request(int Increase) : IRequest<WebRequestResult>;
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
                // Check if user is authenticated, otherwise return error
                if (!_currentCredentials.IsLoggedIn)
                {
                    // TODO: This could be a more specific error
                    return new WebRequestResult(WebRequestResultStatus.Failure);
                }

                if (request.Increase < 0)
                {
                    // TODO: This could be a more specific error
                    return new WebRequestResult(WebRequestResultStatus.Failure);
                }

                // Send request to server
                var result = await _mediator.Send(new Web.IncreaseQuantity.Request(request.Increase), cancellationToken);

                // If successful result, just return back the new quantity, no need to send back entire CustomerModel object
                if (result.Status == WebRequestResultStatus.Success)
                {
                    result.Content = ((ICustomerModel)result.Content).Quantity;
                }

                return result;
            }
        }
    }
}
