using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector.WebAPI;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SimpleInjector.IntegrationTests
{
    public class MyIntegrationTest
    {
        [Fact]
        public async Task Test1()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    // Works, but changes nothing
                    webHost.UseStartup<Startup>();
                });

            var host = await hostBuilder.StartAsync();

            var client = host.GetTestClient();

            // Will get 200
            var response = await client.GetAsync("/weatherforecast");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test2()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    // DI Swap works, but Controllers don't
                    webHost.UseStartup<FakeStartup>();
                });

            var host = await hostBuilder.StartAsync();

            var client = host.GetTestClient();

            // Will get 404
            var response = await client.GetAsync("/weatherforecast");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

    public class EventControllerTests : IClassFixture<EventControllerTests.Factory<FakeStartup>>
    {
        private readonly WebApplicationFactory<FakeStartup> _factory1;
        private readonly WebApplicationFactory<FakeStartup> _factory2;

        public EventControllerTests(Factory<FakeStartup> factory)
        {
            _factory1 = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("SimpleInjector.WebAPI");

                builder.ConfigureTestServices(services =>
                {
                    // Might work?
                    services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
                });

            });

            _factory2 = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("SimpleInjector.WebAPI");

                builder.ConfigureTestServices(services =>
                {
                    // Does not work
                    services.AddMvc().AddApplicationPart(typeof(FakeStartup).Assembly);
                });

            });
        }

        [Fact]
        public async Task Test3()
        {
            var client = _factory1.CreateClient();

            // Will get System.InvalidOperationException from SimpleInjector
            var response = await client.GetAsync("/weatherforecast");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test4()
        {
            var client = _factory2.CreateClient();

            // Will get 404
            var response = await client.GetAsync("/weatherforecast");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public class Factory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
        {
            protected override IWebHostBuilder CreateWebHostBuilder()
            {
                return WebHost.CreateDefaultBuilder(null)
                    .UseStartup<TEntryPoint>();
            }
        }
    }
}
