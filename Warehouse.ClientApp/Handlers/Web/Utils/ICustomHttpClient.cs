#nullable enable
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.ClientApp.Handlers.Web.Utils
{
    public interface ICustomHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
