using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;

namespace Warehouse.ClientApp.Handlers.Web.Utils
{
    public static class HttpRequestMessageSecurityExtensions
    {
        public static void AddBasicAuthentication(this HttpRequestMessage httpRequest, string name, SecureString password)
        {
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes($"{name}:{SecureStringToString(password)}")));
        }

        private static string SecureStringToString(SecureString input)
        {
            return new NetworkCredential(string.Empty, input).Password;
        }
    }
}
