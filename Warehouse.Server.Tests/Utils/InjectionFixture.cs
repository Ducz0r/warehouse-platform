using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Warehouse.Server.Data;

namespace Warehouse.Server.Tests.Utils
{
    public class InjectionFixture : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly IDataContext _dataContext;

        public InjectionFixture()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = _server.CreateClient();
            _dataContext = (IDataContext)_server.Services.GetService(typeof(IDataContext));
        }

        public IServiceProvider ServiceProvider => _server.Host.Services;

        public HttpClient Client { get => _client; }
        public IDataContext DataContext { get => _dataContext; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _server.Dispose();
                _client.Dispose();
            }
        }
    }
}
