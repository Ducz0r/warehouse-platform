using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Warehouse.Server.Data;
using Warehouse.Server.Tests.Utils;
using Xunit;

namespace Warehouse.Server.Tests.Endpoints
{
    public class GetCustomersTests : IClassFixture<InjectionFixture>
    {
        private readonly InjectionFixture _injection;
        private readonly HttpClient _client;
        private readonly IDataContext _dataContext;

        public GetCustomersTests(InjectionFixture injection)
        {
            _injection = injection;
            _client = _injection.Client;
            _dataContext = _injection.DataContext;
        }

        [Fact]
        public async Task Unauthorized_Access()
        {
            var exception = await Record.ExceptionAsync(async () =>
            {
                var response = await _client.GetAsync("/customers");
                Assert.NotNull(response);
                Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Gets_Customers()
        {
            await _dataContext.SeedMultipleCustomers();

            var exception = await Record.ExceptionAsync(async () =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, "customers");
                httpRequest.AddBasicAuthentication();

                var response = await _client.SendAsync(httpRequest);

                Assert.NotNull(response);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

                var responseJson = JArray.Parse(await response.Content.ReadAsStringAsync());
                Assert.NotEmpty(responseJson);

                foreach (JToken token in responseJson.Children())
                {
                    AssertExtra.ValidCustomerJObject((JObject)token);
                }
            });

            Assert.Null(exception);
        }
    }
}
