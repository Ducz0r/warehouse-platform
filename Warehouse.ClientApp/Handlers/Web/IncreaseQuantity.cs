using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.ClientApp.Common;
using Warehouse.ClientApp.Handlers.Web.Utils;
using Warehouse.ClientApp.Models;

namespace Warehouse.ClientApp.Handlers.Web
{
    public class IncreaseQuantity
    {
        public record Request(int Increase) : IRequest<WebRequestResult>;
        public class Handler : IRequestHandler<Request, WebRequestResult>
        {
            private readonly ICustomHttpClient _httpClient;
            private readonly ICurrentCredentials _currentCredentials;

            public Handler(ICustomHttpClient httpClient, ICurrentCredentials currentCredentials)
            {
                _httpClient = httpClient;
                _currentCredentials = currentCredentials;
            }

            public async Task<WebRequestResult> Handle(Request request, CancellationToken cancellationToken)
            {
                var customerId = _currentCredentials.Id;

                HttpResponseMessage response;
                try
                {
                    // Send a "POST /customers/increase-quantity" request with body with quantity
                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"customers/{customerId}/increase-quantity");
                    httpRequest.AddBasicAuthentication(_currentCredentials.Name, _currentCredentials.Password);

                    var contentJson = JObject.FromObject(new { increase = request.Increase });
                    httpRequest.Content = new StringContent(contentJson.ToString(), Encoding.UTF8, "application/json");

                    response = await _httpClient.SendAsync(httpRequest, cancellationToken);
                }
                catch (Exception)
                {
                    // Upon pre-configured timeout, HTTP Client will simply throw Exception
                    response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
                }

                object content = null;
                WebRequestResultStatus status;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content, read id from it
                    content = JsonConvert.DeserializeObject<CustomerModel>(await response.Content.ReadAsStringAsync(cancellationToken));
                    status = WebRequestResultStatus.Success;
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.ServiceUnavailable:
                        case HttpStatusCode.BadGateway:
                        case HttpStatusCode.GatewayTimeout:
                        case HttpStatusCode.InternalServerError:
                            // If 5xx error, set response to "server unavailable"
                            status = WebRequestResultStatus.ServerUnavailable;
                            break;
                        default:
                            // Otherwise, some other failure
                            status = WebRequestResultStatus.Failure;
                            break;
                    }
                }

                return new WebRequestResult(status, content);
            }
        }
    }
}
