using Newtonsoft.Json.Linq;
using System.Linq;
using Xunit;

namespace Warehouse.Server.Tests.Utils
{
    public static class AssertExtra
    {
        public static void ValidCustomerJObject(JObject value)
        {
            Assert.All(value.Children(), t => Assert.Equal(typeof(JProperty), t.GetType()));
            Assert.Equal(3, value.Children().Count());
            Assert.NotNull(value.Children<JProperty>()["id"]);
            Assert.NotNull(value.Children<JProperty>()["name"]);
            Assert.NotNull(value.Children<JProperty>()["quantity"]);
        }
    }
}
