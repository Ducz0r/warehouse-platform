using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Warehouse.Server.Tests.Utils
{
    public static class HttpRequestMessageSecurityExtensions
    {
        public static void AddBasicAuthentication(this HttpRequestMessage httpRequest)
        {
            httpRequest.AddBasicAuthentication(TestConstants.TEST_CUSTOMER_NAME, TestConstants.TEST_CUSTOMER_PASSWORD);
        }

        public static void AddBasicAuthentication(this HttpRequestMessage httpRequest, string name, string password)
        {
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes($"{name}:{password}")));
        }
    }
}
