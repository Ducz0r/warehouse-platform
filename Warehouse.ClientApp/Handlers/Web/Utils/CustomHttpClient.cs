using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;

namespace Warehouse.ClientApp.Handlers.Web.Utils
{
    public class CustomHttpClient : HttpClient, ICustomHttpClient
    {
        public CustomHttpClient() : base()
        {
            BaseAddress = new Uri("https://localhost:5001");
            Timeout = TimeSpan.FromSeconds(3);
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
