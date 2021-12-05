using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Server.Data;
using Warehouse.Server.Tests.Utils;
using Xunit;

namespace Warehouse.Server.Tests.Endpoints
{
    public class IncreaseQuantityTests : IClassFixture<InjectionFixture>
    {
        private readonly InjectionFixture _injection;
        private readonly HttpClient _client;
        private readonly IDataContext _dataContext;

        public IncreaseQuantityTests(InjectionFixture injection)
        {
            _injection = injection;
            _client = _injection.Client;
            _dataContext = _injection.DataContext;
        }

        [Fact]
        public async Task Unauthorized_Access()
        {
            var customer = await _dataContext.SeedSingleCustomer();

            var exception = await Record.ExceptionAsync(async () =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"customers/{customer.Id}/increase-quantity");
                var response = await _client.SendAsync(httpRequest);
                Assert.NotNull(response);
                Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Wrong_Content_Body()
        {
            var customer = await _dataContext.SeedSingleCustomer();

            var exception = await Record.ExceptionAsync(async () =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"customers/{customer.Id}/increase-quantity");
                httpRequest.AddBasicAuthentication();
                var response = await _client.SendAsync(httpRequest);
                Assert.NotNull(response);
                Assert.Equal(System.Net.HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Not_Found()
        {
            var customer = await _dataContext.SeedSingleCustomer();
            var increaseQty = 5;

            var exception = await Record.ExceptionAsync(async () =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"customers/{Guid.NewGuid()}/increase-quantity");
                httpRequest.AddBasicAuthentication();

                var contentJson = JObject.FromObject(new { increase = increaseQty });
                httpRequest.Content = new StringContent(contentJson.ToString(), Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(httpRequest);
                Assert.NotNull(response);
                Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Successful_Increase()
        {
            var customer = await _dataContext.SeedSingleCustomer();
            var increaseQty = 5;

            var exception = await Record.ExceptionAsync(async () =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"customers/{customer.Id}/increase-quantity");
                httpRequest.AddBasicAuthentication();

                var contentJson = JObject.FromObject(new { increase = increaseQty });
                httpRequest.Content = new StringContent(contentJson.ToString(), Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(httpRequest);
                Assert.NotNull(response);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                AssertExtra.ValidCustomerJObject(responseJson);
                Assert.Equal(increaseQty, responseJson.Value<int>("quantity"));
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Multiple_Increases_Total_Count_Valid()
        {
            var customer = await _dataContext.SeedSingleCustomer();
            var increaseQties = new int[] { 5, 8, 9, 11, 13 };
            var total = increaseQties.Sum();

            var exception = await Record.ExceptionAsync(async () =>
            {
                HttpResponseMessage response = null;
                JObject responseJson = null;

                foreach (var increaseQty in increaseQties)
                {
                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"customers/{customer.Id}/increase-quantity");
                    httpRequest.AddBasicAuthentication();

                    var contentJson = JObject.FromObject(new { increase = increaseQty });
                    httpRequest.Content = new StringContent(contentJson.ToString(), Encoding.UTF8, "application/json");

                    response = await _client.SendAsync(httpRequest);
                    responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                    Assert.NotNull(response);
                    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
                }

                // Verify last response
                AssertExtra.ValidCustomerJObject(responseJson);
                Assert.Equal(total, responseJson.Value<int>("quantity"));
            });

            Assert.Null(exception);
        }
    }
}
