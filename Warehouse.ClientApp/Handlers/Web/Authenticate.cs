using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.ClientApp.Handlers.Web.Utils;
using Warehouse.ClientApp.Models;

namespace Warehouse.ClientApp.Handlers.Web
{
    public class Authenticate
    {
        public record Request(string Name, SecureString Password) : IRequest<WebRequestResult>;
        public class Handler : IRequestHandler<Request, WebRequestResult>
        {
            private readonly ICustomHttpClient _httpClient;

            public Handler(ICustomHttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<WebRequestResult> Handle(Request request, CancellationToken cancellationToken)
            {
                HttpResponseMessage response;
                try
                {
                    // Query "POST /customers/find/by-name", to check out if authentication works,
                    // and to get back the customer GUID
                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, "customers/find/by-name");
                    httpRequest.AddBasicAuthentication(request.Name, request.Password);

                    var contentJson = JObject.FromObject(new { name = request.Name });
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
